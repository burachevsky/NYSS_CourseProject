using Microsoft.VisualStudio.TestTools.UnitTesting;
using CrypterCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrypterCore.Tests
{
    [TestClass()]
    public class VigenereTableTests
    {
        /*[TestMethod()]
        public void VigenereTableTest()
        {
            Assert.Fail();
        }*/

        private static VigenereTable table = new VigenereTable(Alphabets.RUSSIAN);

        [TestMethod]
        public void EncryptTest1()
        {
            char input = 'б';
            char key = 'б';
            char res = 'в';
            Assert.AreEqual(res, table.EncryptChar(input, key));
        }

        [TestMethod]
        public void EncryptTest2()
        {
            char input = 'б';
            char key = 'д';
            char res = 'е';
            Assert.AreEqual(res, table.EncryptChar(input, key));
        }

        [TestMethod]
        public void EncryptTest3()
        {
            char input = 'г';
            char key = 'ё';
            char res = 'и';
            Assert.AreEqual(res, table.EncryptChar(input, key));
        }

        [TestMethod]
        public void EncryptTest4()
        {
            char input = 'ю';
            char key = 'я';
            char res = 'э';
            Assert.AreEqual(res, table.EncryptChar(input, key));
        }

        [TestMethod]
        public void EncryptTest5()
        {
            char input = 'я';
            char key = 'я';
            char res = 'ю';
            Assert.AreEqual(res, table.EncryptChar(input, key));
        }

        [TestMethod]
        public void EncryptTest6()
        {
            char input = 'л';
            char key = 'з';
            char res = 'у';
            Assert.AreEqual(res, table.EncryptChar(input, key));
        }

        [TestMethod]
        public void DecryptTest1()
        {
            char input = 'б';
            char key = 'с';
            char res = 'п';
            Assert.AreEqual(res, table.DecryptChar(input, key));
        }

        [TestMethod]
        public void DecryptTest2()
        {
            char input = 'л';
            char key = 'з';
            char res = 'д';
            Assert.AreEqual(res, table.DecryptChar(input, key));
        }
    }
}