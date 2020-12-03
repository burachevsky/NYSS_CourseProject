using System;
using System.IO;
using CrypterMobile.Services;
using System.Text;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Widget;
using CrypterMobile.Views;
using Plugin.FilePicker;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Toast = Android.Widget.Toast;

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
                    var dir = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "crypter");

                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    var path = Path.Combine(dir, fileName);

                    if (fileName.EndsWith(".docx"))
                    {
                        File.WriteAllBytes(path, Formats.ConvertToDocx(text));
                    }
                    else if (fileName.EndsWith(".txt"))
                    {
                        File.WriteAllText(path, text);
                    }

                    Alert.LongAlert($"Файл сохранен {path}");
                    OnSuccess?.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception choosing file: " + ex);
                    Alert.LongAlert(ex.Message);
                }
            };

            await Shell.Current.GoToAsync(nameof(GetFilePage));
        }

        public async void OpenReadFileDialog(Action<string> OnSuccess = null)
        {
            try
            {
                using (var fileData = await CrossFilePicker.Current.PickFile(new[] { ".txt", ".docx" }))
                {
                    if (fileData == null)
                        return;

                    var fileName = fileData.FileName;

                    string contents = null;

                    if (fileName.EndsWith(".docx"))
                    {
                        contents = Formats.GetTextFromDocx(fileData.DataArray);
                    }
                    else if (fileName.EndsWith(".txt"))
                    {
                        contents = Encoding.UTF8.GetString(fileData.DataArray);
                    }

                    if (contents != null)
                    {
                        OnSuccess?.Invoke(contents);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception choosing file: " + ex);
                Alert.LongAlert(ex.Message);
            }
        }


    }
}