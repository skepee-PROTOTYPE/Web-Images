using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Collections.Generic;
using System;
using Azure.Identity;
using Azure.Storage.Sas;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace WebImage.Models
{
    internal static class AzureStorage
    {
        private static StorageCredentials storageCredentials = new StorageCredentials(Settings.StorageAccount, Settings.Secrets);
        private static CloudStorageAccount cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);

        public static async Task AddFilesAsync(string filename, string container)
        {
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);

            using (var fileStream = File.OpenRead(filename))
            {
                var file = Path.GetFileName(filename);
                var blockBlob = cloudBlobContainer.GetBlockBlobReference(file);

                await blockBlob.UploadFromStreamAsync(fileStream);
            }
        }


        public static async void CreateStorage(string container)
        {
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);

            await cloudBlobContainer.CreateIfNotExistsAsync();

            await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Off
            });
        }


        public static async Task<byte[]> DownloadFileFromBlob(string blobReferenceKey, string container)
        {
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
            var blockBlob = cloudBlobContainer.GetBlockBlobReference(blobReferenceKey);

            using (var ms = new MemoryStream())
            {
                bool exists = await blockBlob.ExistsAsync();

                if (exists)
                {
                    await blockBlob.DownloadToStreamAsync(ms);
                }
                return ms.ToArray();
            }
        }



        public static async Task<Dictionary<string, byte[]>> ListBlobsFlatListingAsync(string container, int? segmentSize)
        {
            Dictionary<string, byte[]> res = new Dictionary<string, byte[]>();

            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(container);

            BlobContinuationToken continuationToken = null;
            CloudBlob blob;

            try
            {
                do
                {
                    BlobResultSegment resultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(string.Empty, true, BlobListingDetails.Metadata, segmentSize, continuationToken, null, null);

                    foreach (var blobItem in resultSegment.Results)
                    {
                        blob = (CloudBlob)blobItem;
                        var blockBlob = cloudBlobContainer.GetBlockBlobReference(container);

                        using (var ms = new MemoryStream())
                        {
                            if (await blockBlob.ExistsAsync())
                            {
                                await blockBlob.DownloadToStreamAsync(ms);
                            }
                            res.Add(blob.Name, ms.ToArray());
                        }
                    }

                    continuationToken = resultSegment.ContinuationToken;
                } while (continuationToken != null);
            }
            catch (StorageException e)
            {
                throw;
            }

            return res;
        }

        private async static Task<Uri> GetUserDelegationSasBlob(string containerName, string blobName)
        {
            // Construct the blob endpoint from the account name.
            string blobEndpoint = string.Format("https://{0}.blob.core.windows.net", Settings.StorageAccount);


            var credential = new ClientSecretCredential(
                                               Settings.TenantId,
                                               Settings.ClientId,
                                               Settings.ClientSecret,
                                               new TokenCredentialOptions());

            // Create a new Blob service client with Azure AD credentials.  
            BlobServiceClient blobClient = new BlobServiceClient(new Uri(blobEndpoint), credential);

            // Get a user delegation key for the Blob service that's valid for seven days.
            // You can use the key to generate any number of shared access signatures over the lifetime of the key.
            UserDelegationKey key = await blobClient.GetUserDelegationKeyAsync(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddDays(7));

            // Create a SAS token that's valid for one hour.
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };

            // Specify read permissions for the SAS.
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            // Use the key to get the SAS token.
            string sasToken = sasBuilder.ToSasQueryParameters(key, Settings.StorageAccount).ToString();

            // Construct the full URI, including the SAS token.
            UriBuilder fullUri = new UriBuilder()
            {
                Scheme = "https",
                Host = string.Format("{0}.blob.core.windows.net", Settings.StorageAccount),
                Path = string.Format("{0}/{1}", containerName, blobName),
                Query = sasToken
            };

            return fullUri.Uri;
        }

        public static async Task<string> BlobUrlAsync(string containerName, string blobName)
        {
            var blobUrl = await GetUserDelegationSasBlob(containerName, blobName);
            return blobUrl.AbsoluteUri;
        }
    }
}
