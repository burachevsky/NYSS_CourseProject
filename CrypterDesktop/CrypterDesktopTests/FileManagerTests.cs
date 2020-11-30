using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrypterDesktop;
using System;
using System.Collections.Generic;
using System.Linq;
using CrypterCore;

namespace CrypterDesktopTests
{
    [TestClass]
    public class FileManagerTests
    {
        [TestMethod]
        public void Test1()
        {
            var cipher = new VigenereCipher("Скоприон", Alphabets.RUSSIAN);
            var text = "Какой-то текст просто для теста";
            var file = "tmp.docx";

            var fileManager = new FileManager(null);

            fileManager.Save(file, cipher.Encrypt(text));

            var encryptedText = fileManager.ReadFile(file);
            var decryptedEncryptedText = cipher.Decrypt(encryptedText);

            Assert.IsTrue(text.Equals(decryptedEncryptedText));
        }

        [TestMethod]
        public void Test2()
        {
            var cipher = new VigenereCipher("Скоприон", Alphabets.RUSSIAN);
            var text = "Какой-то текст просто для теста";
            var file = "tmp.txt";

            var fileManager = new FileManager(null);

            fileManager.Save(file, cipher.Encrypt(text));

            var encryptedText = fileManager.ReadFile(file);

            Assert.IsTrue(text.Equals(cipher.Decrypt(encryptedText)));
        }
    }
}