using System;
using System.Collections.Generic;
using System.Text;

namespace CrypterMobile.Services
{
    public class DirectoryListItem
    {
        public string Name { get; }
        public string ImageSource { get; }

        public bool IsDirectory { get; }

        public DirectoryListItem(string name, bool isDirectory)
        {
            Name = name;
            IsDirectory = isDirectory;
            ImageSource = isDirectory
                ? "Resources/drawable/icon_folder_48.png"
                : name.EndsWith(".txt") 
                    ? "Resources/drawable/icon_txt_60.png"
                    : name.EndsWith(".docx") 
                        ? "Resources/drawable/icon_word_96.png" 
                        : "Resources/drawable/icon_file_50.png";
        }
    }
}
