using Firebase.Storage;
using System.IO;
using System.Threading.Tasks;

namespace TidRod.Services.Helper
{
    public class FSHelper
    {
        private readonly FirebaseStorage
            firebaseStorage =
                new FirebaseStorage(AppSettings.FIREBASE_STORAGE_URL);

        public string RootName { get; set; }

        public FSHelper()
        {
            this.RootName = AppSettings.FIREBASE_STORAGE_ROOT;
        }

        public async Task<string> UploadFile(Stream fileStream, string fileName)
        {
            var imageUrl =
                await firebaseStorage
                    .Child(RootName)
                    .Child(fileName)
                    .PutAsync(fileStream);
            return imageUrl;
        }

        public async Task<string>
        UploadFile(Stream fileStream, string fileName, string root)
        {
            var imageUrl =
                await firebaseStorage
                    .Child(root)
                    .Child(fileName)
                    .PutAsync(fileStream);
            return imageUrl;
        }

        public async Task<string> GetFile(string fileName)
        {
            return await firebaseStorage
                .Child(RootName)
                .Child(fileName)
                .GetDownloadUrlAsync();
        }

        public async Task DeleteFile(string fileName)
        {
            await firebaseStorage.Child(RootName).Child(fileName).DeleteAsync();
        }
    }
}
