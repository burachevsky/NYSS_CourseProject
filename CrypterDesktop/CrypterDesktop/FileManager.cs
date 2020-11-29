using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using Microsoft.Win32;
using Spire.Doc;

namespace CrypterDesktop
{
    public class FileManager
    {
        private MainWindow mainWindow;

        public FileManager(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void OpenReadFileDialog()
        {
            App.Run(() =>
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.CheckPathExists = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|Document Files (*.docx)|*.docx";
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == true)
                {
                    var text = ReadFile(openFileDialog.FileName);
                    mainWindow.SetDefaultConfiguration();
                    mainWindow.TextBox_Input.Text = text;
                    mainWindow.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }
            });
        }

        public string ReadFile(string path)
        {
            var extension = path.Substring(path.LastIndexOf('.'));

            if (extension.Equals(".docx"))
            {
                return ReadDocx(path);
            }
            else if (extension.Equals(".txt"))
            {
                return File.ReadAllText(path);
            }

            throw new ArgumentException();
        }

        private string ReadDocx(string path)
        {
            string text;

            using (var doc = new Document())
            {
                File.WriteAllBytes("temp.docx", File.ReadAllBytes(path));
                doc.LoadFromFile("temp.docx");
                text = doc.GetText();
            }
            
            File.Delete("temp.docx");

            return text.Substring(71);
        }
    }
}