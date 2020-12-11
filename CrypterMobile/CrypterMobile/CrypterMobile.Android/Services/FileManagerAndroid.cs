using CrypterMobile.Services;
using CrypterMobile.ViewModels;
using CrypterMobile.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrypterMobile.Droid.Services
{
    public class FileManagerAndroid : IFileManager
    {
        private Encoding CurrentEncoding { get; set; }

        public event Action<string> OnFileNameChoosed;

        private string[] Encodings { get; }
        private Dictionary<string, EncodingInfo> EncodingsMap { get; }

        private string currentEncodingName;
        public string CurrentEncodingName
        {
            get => currentEncodingName;
            set
            {
                currentEncodingName = value;
                CurrentEncoding = EncodingsMap[value].GetEncoding();
            }
        }

        public FileManagerAndroid()
        {
            var encodings = Encoding.GetEncodings();

            EncodingsMap = new Dictionary<string, EncodingInfo>();

            foreach (var enc in encodings)
            {
                EncodingsMap.TryAdd(enc.Name, enc);
            }

            Encodings = EncodingsMap.Keys.ToArray();
            CurrentEncodingName = "utf-8";
        }

        public void FileNameChoosed(string fileName) => OnFileNameChoosed?.Invoke(fileName);

        public async void OpenSaveFileDialog(string text, Action OnSuccess = null)
        {
            OnFileNameChoosed = (file) =>
            {
                if (file == null)
                    return;

                try
                {
                    if (file.EndsWith(".docx"))
                    {
                        File.WriteAllBytes(file, Formats.ConvertToDocx(text));
                    }
                    else if (file.EndsWith(".txt"))
                    {
                        File.WriteAllText(file, text, CurrentEncoding);
                    }

                    Alert.LongAlert($"Файл сохранен по адресу: {file}");
                    OnSuccess?.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception choosing file: " + ex);
                    Alert.LongAlert(ex.Message);
                }
            };

            var filePicker = Shell.Current.GoToAsync(nameof(GetFilePage));

            GetFileViewModel.Current.ApplyMode(GetFileViewModel.GetFileMode.SaveAs);

            await filePicker;
        }

        public async void OpenReadFileDialog(Action<string> OnSuccess = null)
        {
            OnFileNameChoosed = (file) =>
            {
                try
                {
                    if (file == null)
                        return;

                    string contents = null;

                    if (file.EndsWith(".docx"))
                    {
                        contents = Formats.GetTextFromDocx(File.ReadAllBytes(file));
                    }
                    else if (file.EndsWith(".txt"))
                    {

                        var bytes = File.ReadAllBytes(file);
                        contents = CurrentEncoding.GetString(bytes);
                    }

                    if (contents != null)
                    {
                        OnSuccess?.Invoke(contents);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception choosing file: " + e.StackTrace);
                    Alert.LongAlert(e.GetType() + ":\n" + e.Message);
                }
            };

            var filePicker = Shell.Current.GoToAsync(nameof(GetFilePage));

            GetFileViewModel.Current.ApplyMode(GetFileViewModel.GetFileMode.Open);

            await filePicker;
        }

        public void CreateFolder(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
                Alert.LongAlert("Папка создана: " + path);
            }
            catch (Exception e)
            {
                Alert.LongAlert(e.Message);
            }
        }

        public Task<(string, List<DirectoryListItem>)> GetStartDirectory()
        {
            return GetDirectory(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
        }

        public Task<(string, List<DirectoryListItem>)> GetDirectory(string dir)
        {
            return Task.Run(() =>
            {
                if (!dir.EndsWith('/'))
                {
                    dir += '/';
                }

                return (dir, GetItemsInDirectory(dir));
            });
        }

        public bool IsRoot(string dir)
        {
            if (dir.EndsWith('/'))
            {
                dir = dir.Substring(0, dir.Length - 1);
            }

            return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath.Equals(dir);
        }

        public List<DirectoryListItem> GetItemsInDirectory(string path)
        {
            return Directory.EnumerateFiles(path)
                .Select(s => new DirectoryListItem(s.Substring(s.LastIndexOf('/') + 1), false))
                .Concat
                (
                    Directory.EnumerateDirectories(path)
                        .Select(s => new DirectoryListItem(s.Substring(s.LastIndexOf('/') + 1), true))
                )
                .OrderBy(s => s.Name)
                .ToList();
        }

        public string[] GetAvailableEncodings()
        {
            return Encodings;
        }
    }
}