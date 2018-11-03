using BlobLib;
using System;
using System.IO;

namespace BlobStorageUploadAndCopyFile
{
    class Program
    {
        private const string StorageConnectionString = ""; // TODO key. push前に削除
        private const string ContainerName = "samplecontainer";

        static void Main(string[] args)
        {
            args = SetArgDummy(); // TODO debug

            if (!IsValid(args))
            {
                Environment.Exit(-1);
            }

            using (var stream = new FileStream(args[0], FileMode.Open, FileAccess.Read))
            {
                var blob = new BlobFileUpdator(StorageConnectionString, ContainerName);
                blob.CreateAndCopyFileAsync(args[0], args[1], stream, int.Parse(args[2])).Wait();
            }

        }

        private static bool IsValid(string[] args)
        {
            return true; // TODO あとで
        }

        static string[] SetArgDummy()
        {
            // arg0 local file name
            // arg1 file count
            return new[] { @"C:\wk\testdata.txt", "test1", "30" };
        }
    }
}
