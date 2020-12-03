using System;
using System.Collections.Generic;
using System.Text;

namespace CrypterMobile.Services
{
    public interface IFileManager
    {
        public void FileNameChoosed(string fileName);

        public void OpenSaveFileDialog(string text, Action OnSuccess = null);

        public void OpenReadFileDialog(Action<string> OnSuccess = null);
    }
}
