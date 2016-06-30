using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            // This is a sample program which demonstrates using the APIs.

            // ** IMPORTANT **
            // If you get errors from the development storage account, make sure
            // you're running the storage emulator version 4.2.0 or higher.
            // Version 4.2.0 can be downloaded from:
            // http://go.microsoft.com/fwlink/p/?linkid=618715&clcid=0x409
            // Hit the orange "Download" button and select
            // the file MicrosoftAzureStorageEmulator.msi.

            var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("key-container");

            // The container must exist before calling the DataProtection APIs.
            // The specific file within the container does not have to exist,
            // as it will be created on-demand.

            container.CreateIfNotExists();

            // Configure

            ServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection()
                .PersistKeysToAzureBlobStorage(container, "keys.xml");
            
            var services = serviceCollection.BuildServiceProvider();

            // Run a sample payload

            var protector = services.GetDataProtector("sample-purpose");
            var protectedData = protector.Protect("Hello world!");
            Console.WriteLine(protectedData);
        }
    }
}
