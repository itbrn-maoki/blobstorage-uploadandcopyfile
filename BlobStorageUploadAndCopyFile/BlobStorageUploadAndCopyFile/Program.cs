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
            //args = SetArgDummy(); // TODO debug

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
            if(args.Length != 3)
            {
                Console.WriteLine("コマンドライン引数がたりません。");
                return false;
            }

            if (string.IsNullOrWhiteSpace(args[0]))
            {
                Console.WriteLine("第1引数にアップロードするファイルを指定してください。");
                return false;
            }

            if (!File.Exists(args[0])){
                Console.WriteLine("第1引数に指定されたファイルが存在しません。");
                return false;
            }

            if (string.IsNullOrWhiteSpace(args[1]))
            {
                Console.WriteLine("第2引数にblob storageの格納先のパスを指定してください。");
                return false;
            }

            if (string.IsNullOrWhiteSpace(args[2]))
            {
                Console.WriteLine("第3引数にファイルの数を指定してください。");
                return false;
            }

            if(!int.TryParse(args[2], out int num))
            {
                Console.WriteLine("第3引数にファイルの数を数字で指定してください。");
                return false;
            }

            return true;
        }

        static string[] SetArgDummy()
        {
            // arg0 local file name
            // arg1 file count
            return new[] { @"C:\wk\testdata.txt", "test1", "30" };
        }
    }
}
