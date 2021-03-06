﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CrypterMobile.Services
{
    public interface IFileManager
    {
        public string FileNameChoosed(string file);

        public string CurrentEncodingName { get; set; }

        public void OpenSaveFileDialogAsync(string text, Action OnSuccess = null);

        public void OpenReadFileDialogAsync(Action<string> OnSuccess = null);

        public Task<(string, List<DirectoryListItem>)> GetStartDirectory();

        public List<DirectoryListItem> GetItemsInDirectory(string path);

        public Task<(string, List<DirectoryListItem>)> GetDirectory(string dir);

        public bool IsRoot(string dir);

        public void CreateFolder(string path);

        public string[] GetAvailableEncodings();
    }
}