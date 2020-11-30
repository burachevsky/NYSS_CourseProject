using System;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Text.RegularExpressions;
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
            FileDialog(() =>
            {
                var openFileDialog = new OpenFileDialog();
                openFileDialog.CheckPathExists = true;
                openFileDialog.CheckFileExists = true;
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|Document Files (*.docx)|*.docx";
                openFileDialog.Multiselect = false;
                openFileDialog.ShowReadOnly = true;
                if (openFileDialog.ShowDialog() == true)
                {
                    var text = ReadFile(openFileDialog.FileName);
                    mainWindow.SetDefaultConfiguration();
                    mainWindow.TextBox_Input.Text = text;
                    mainWindow.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }
            });
        }

        public void OpenSaveFileDialog(string text)
        {
            FileDialog(() =>
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|Document Files (*.docx)|*.docx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    Save(saveFileDialog.FileName, text);
                }
            });
        }

        private void FileDialog(Action action)
        {
            App.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        action?.Invoke();
                        break;
                    }
                    catch (Exception e)
                    {
                        var text =
                            $"Не удалось открыть файл.\nВозможно, он занят другим процессом или поврежден.\nПричина ошибки: {e.GetType()}\nХотите попробовать заново?";
                        var btn = MessageBoxButton.YesNo;
                        var result = MessageBox.Show(text, "", btn);
                        if (result == MessageBoxResult.Yes)
                        {
                            continue;
                        }

                        break;
                    }
                }
            });
        }

        public string ReadFile(string path)
        {
            if (path.EndsWith(".docx"))
            {
                return ReadDocx(path);
            }

            if (path.EndsWith(".txt"))
            {
                return File.ReadAllText(path);
            }

            throw new ArgumentException();
        }

        public void Save(string path, string text)
        {
            if (path.EndsWith(".docx"))
            {
                WriteDocs(path, text);
                return;
            }

            if (path.EndsWith(".txt"))
            {
                File.WriteAllText(path, text);
                return;
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

            var shift = text.EndsWith("\r\n") ? 2 : 0;

            return text.Substring(71, text.Length - 71 - shift);
        }

        private void WriteDocs(string path, string text)
        {
            using (var doc = new Document())
            {
                var section = doc.AddSection();
                var paragraph = section.AddParagraph();
                paragraph.AppendText(text);

                doc.SaveToFile(path, FileFormat.Docx);

                using (var stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Update))
                    {
                        string resultDoc;

                        using (var xmlDoc = zipArchive.GetEntry("word/document.xml").Open())
                        {
                            using (var streamReader = new StreamReader(xmlDoc))
                            {
                                var document = streamReader.ReadToEnd();
                                resultDoc = document.Remove(document.IndexOf("<w:p>"), 192);
                            }
                        }

                        using (var replacedFile = zipArchive.GetEntry("word/document.xml").Open())
                        {
                            replacedFile.SetLength(resultDoc.Length);
                            using (var writer = new StreamWriter(replacedFile))
                            {
                                writer.Write(resultDoc);
                            }
                        }
                    }
                }
            }
        }
    }
}