using System;
using System.IO;
using System.IO.Compression;
using Spire.Doc;

namespace CrypterMobile.Services
{
    public static class Formats
    {
        public static string GetTextFromDocx(byte[] source)
        {
            string text;
            using (var doc = new Document())
            {
                using (var stream = new MemoryStream(source))
                {
                    doc.LoadFromStream(stream, FileFormat.Docx);
                    text = doc.GetText();
                }
            }

            var shift = text.EndsWith("\r\n") ? 2 : 0;
            return text.Substring(71, text.Length - 71 - shift);
        }

        public static byte[] ConvertToDocx(string text)
        {
            using var doc = new Document();
            var tmpFile = Path.Combine
            (
                Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                "tmp.docx"
            );

            var section = doc.AddSection();
            var paragraph = section.AddParagraph();
            paragraph.AppendText(text);
            doc.SaveToFile(tmpFile, FileFormat.Docx);
            File.SetAttributes(tmpFile, FileAttributes.Normal);
            using (var stream = new FileStream(tmpFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                using (var zipArchive = new ZipArchive(stream, ZipArchiveMode.Update))
                {
                    string resultDoc;

                    using (var xmlDoc = zipArchive.GetEntry("word/document.xml")?.Open())
                    {
                        using (var streamReader = new StreamReader(xmlDoc))
                        {
                            var document = streamReader.ReadToEnd();
                            resultDoc = document.Remove(document.IndexOf("<w:p>", StringComparison.Ordinal), 192);
                        }
                    }

                    using (var replacedFile = zipArchive.GetEntry("word/document.xml")?.Open())
                    {
                        replacedFile.SetLength(resultDoc.Length);
                        using (var writer = new StreamWriter(replacedFile))
                        {
                            writer.Write(resultDoc);
                        }
                    }
                }
            }

            var bytes = File.ReadAllBytes(tmpFile);
            File.Delete(tmpFile);
            return bytes;
        }
    }
}
