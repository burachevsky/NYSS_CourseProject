using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrypterMobile.Services
{
    public interface IFileManager
    {
        public void FileNameChoosed(string fileName);

        public void OpenSaveFileDialog(string text, Action OnSuccess = null);

        public void OpenReadFileDialog(Action<string> OnSuccess = null);

        public Task<(string, List<DirectoryListItem>)> GetStartDirectory();

        public Task<List<DirectoryListItem>> GetItemsInDirectory(string path);

        public Task<(string, List<DirectoryListItem>)> GetDirectory(string dir);

        public bool IsRoot(string dir);
    }
}