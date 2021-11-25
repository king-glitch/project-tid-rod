using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TidRod.Models;
using TidRod.Services.Helper;
using Xamarin.Forms;

namespace TidRod.Utils
{
    public class TidRodUtilitiles
    {
        private static readonly FSHelper helper = new FSHelper();
        public static string StringSha256Hash(string text) =>
        string.IsNullOrEmpty(text) ? string.Empty : BitConverter.ToString(new System.Security.Cryptography.SHA256Managed().ComputeHash(System.Text.Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty);

        public static async Task<string> SaveFileToServer(FileImage _file)
        {
            return await SaveFileToServer(_file, AppSettings.FIREBASE_STORAGE_ROOT);
        }

        public static async Task<string> SaveFileToServer(FileImage _file, string root)
        {
            string _uriFile = "";

            var imageAsBytes = ImageSourceToBytes(_file.Image);
            if (imageAsBytes != null)
            {
                using (var StreamF = new MemoryStream(imageAsBytes))
                {
                    _uriFile = await helper.UploadFile(StreamF, StringSha256Hash(_file.FileName.Split('.')[0]) + _file.FileName.Split('.')[1], root);
                }
            }
            return _uriFile;
        }
        public static byte[] ImageSourceToBytes(ImageSource imageSource)
        {
            StreamImageSource streamImageSource = (StreamImageSource)imageSource;
            var cancellationToken = CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;
            byte[] bytesAvailable = new byte[stream.Length];
            _ = stream.Read(bytesAvailable, 0, bytesAvailable.Length);
            return bytesAvailable;
        }

        public static byte[] StreamToByteArray(Stream source)
        {
            byte[] imageAsBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                source?.CopyTo(memoryStream);
                imageAsBytes = memoryStream.ToArray();
            }

            return imageAsBytes;
        }

        public static bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhone(string phone)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                    return false;
                var r = new Regex(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$");
                return r.IsMatch(phone);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
