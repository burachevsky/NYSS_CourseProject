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

        public event Func<string, string> OnFileNameChoosed;

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

        public string FileNameChoosed(string fileName) => OnFileNameChoosed?.Invoke(fileName);

        public async void OpenSaveFileDialogAsync(string text, Action OnSuccess = null)
        {
            var filePicker = Shell.Current.GoToAsync(nameof(GetFilePage));
            await filePicker;

            GetFileViewModel.Current.ApplyMode(GetFileViewModel.GetFileMode.SaveAs);

            OnFileNameChoosed = file =>
            {
                if (file == null)
                    return null;

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

                    OnSuccess?.Invoke();
                    return $"Файл сохранен по адресу: {file}";
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception choosing file: " + ex);
                    return ex.Message;
                }
            };
        }

        public async void OpenReadFileDialogAsync(Action<string> OnSuccess = null)
        {
            var filePicker = Shell.Current.GoToAsync(nameof(GetFilePage));
            await filePicker;
            GetFileViewModel.Current.ApplyMode(GetFileViewModel.GetFileMode.Open);

            OnFileNameChoosed = file =>
            {
                try
                {
                    if (file == null)
                        return null;

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

                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception choosing file: " + e.StackTrace);
                    return e.GetType() + ":\n" + e.Message;
                }
            };
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