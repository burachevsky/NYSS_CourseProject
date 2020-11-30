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

        public void OpenSaveFileDialog(string text)
        {
            App.Run(() =>
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|Document Files (*.docx)|*.docx";
                if (saveFileDialog.ShowDialog() == true)
                {
                    Save(saveFileDialog.FileName, text);
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

            return text.Substring(71);
        }

        private void WriteDocs(string path, string text)
        {
            using (var doc = new Document())
            {
                var section = doc.AddSection();
                var paragraph = section.AddParagraph();
                paragraph.AppendText(text);

                try
                {
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
                catch (Exception e)
                {
                    MessageBox.Show($"{e.GetType()} {e.StackTrace}");
                }
            }
        }
    }
}