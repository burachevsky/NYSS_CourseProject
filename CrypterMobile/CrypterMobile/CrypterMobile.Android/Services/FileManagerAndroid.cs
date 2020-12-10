using CrypterMobile.Services;
using CrypterMobile.Views;
using Plugin.FilePicker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrypterMobile.ViewModels;
using Xamarin.Forms;

namespace CrypterMobile.Droid.Services
{
    public class FileManagerAndroid : IFileManager
    {
        public event Action<string> OnFileNameChoosed;

        public void FileNameChoosed(string fileName) => OnFileNameChoosed?.Invoke(fileName);

        public async void OpenSaveFileDialog(string text, Action OnSuccess = null)
        {
            OnFileNameChoosed = fileName =>
            {
                if (fileName == null)
                    return;

                try
                {

                    if (fileName.EndsWith(".docx"))
                    {
                        File.WriteAllBytes(fileName, Formats.ConvertToDocx(text));
                    }
                    else if (fileName.EndsWith(".txt"))
                    {
                        File.WriteAllText(fileName, text);
                    }

                    Alert.LongAlert($"Файл сохранен по адресу: {fileName}");
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
            OnFileNameChoosed = file =>
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
                        contents = File.ReadAllText(file);
                    }

                    if (contents != null)
                    {
                        OnSuccess?.Invoke(contents);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception choosing file: " + e.StackTrace);
                    Alert.LongAlert(e.Message);
                }
            };

            var filePicker = Shell.Current.GoToAsync(nameof(GetFilePage));

            GetFileViewModel.Current.ApplyMode(GetFileViewModel.GetFileMode.Open);

            await filePicker;
        }

        public async Task<(string, List<DirectoryListItem>)> GetStartDirectory()
        {
            return await GetDirectory(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
        }

        public async Task<(string, List<DirectoryListItem>)> GetDirectory(string dir)
        {
            if (!dir.EndsWith('/'))
            {
                dir += '/';
            }

            return (dir, await GetItemsInDirectory(dir));
        }

        public bool IsRoot(string dir)
        {
            if (dir.EndsWith('/'))
            {
                dir = dir.Substring(0, dir.Length - 1);
            }

            return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath.Equals(dir);
        }

        public async Task<List<DirectoryListItem>> GetItemsInDirectory(string path)
        {
            return Directory.EnumerateFiles(path)
                .Select(s => new DirectoryListItem(s.Substring(s.LastIndexOf('/') + 1), false))
                .Concat
                (
                    Directory.EnumerateDirectories(path).Select(s => new DirectoryListItem(s.Substring(s.LastIndexOf('/') + 1), true)
                )).ToList();
        }
    }
}