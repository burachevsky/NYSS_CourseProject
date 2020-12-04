using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Spire.Doc;

namespace CrypterDesktop.Services
{
    public class FileManager
    {
        public Task<string> OpenReadFileDialog()
        {
            var task = new Task<string>(() =>
            {
                try
                {
                    var openFileDialog = new OpenFileDialog();
                    openFileDialog.CheckPathExists = true;
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.Filter = "Document Files (*.docx)|*.docx|Text Files (*.txt)|*.txt";
                    openFileDialog.Multiselect = false;
                    openFileDialog.ShowReadOnly = true;
                    if (openFileDialog.ShowDialog() == true)
                    {
                        return ReadFile(openFileDialog.FileName);
                    }
                }
                catch (Exception e)
                {
                    HandleException(e);
                }

                return null;
            });

            task.Start();

            return task;
        }

        public Task OpenSaveFileDialog(string text)
        {
            var task = new Task(() =>
            {
                try
                {
                    var saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Document Files (*.docx)|*.docx|Text Files (*.txt)|*.txt";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        Save(saveFileDialog.FileName, text);
                        MessageBox.Show("Готово");
                    }
                }
                catch (Exception e)
                {
                    HandleException(e);
                }
            });

            task.Start();

            return task;
        }

        private void HandleException(Exception e)
        {
            var text = e is ArgumentException
                ? $"Не выполнить операцию. Формат должен быть .docx или .txt.\nОшибка: {e.GetType()}"
                : $"Не удалось выполнить операцию.\nВозможно, он занят другим процессом или поврежден.\nПричина ошибки: {e.GetType()}\nСтек трейс: {e.StackTrace}";

            MessageBox.Show(text);
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
                File.WriteAllBytes(path, ConvertToDocx(text));
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
                File.Copy(path, "tmp.docx");
                File.SetAttributes("tmp.docx", FileAttributes.Normal);

                doc.LoadFromFile("tmp.docx");
                text = doc.GetText();
            }

            File.Delete("tmp.docx");

            var shift = text.EndsWith("\r\n") ? 2 : 0;

            return text.Substring(71, text.Length - 71 - shift);
        }

        private byte[] ConvertToDocx(string text)
        {
            using (var doc = new Document())
            {
                var section = doc.AddSection();
                var paragraph = section.AddParagraph();
                paragraph.AppendText(text);

                doc.SaveToFile("tmp.docx", FileFormat.Docx);
                File.SetAttributes("tmp.docx", FileAttributes.Normal);

                using (var stream = new FileStream("tmp.docx", FileMode.OpenOrCreate, FileAccess.ReadWrite))
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

                var bytes = File.ReadAllBytes("tmp.docx");
                File.Delete("tmp.docx");

                return bytes;
            }
        }
    }
}