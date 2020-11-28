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
    public class RussianAlphabetTests
    {
        private IAlphabet alphabet = Alphabets.RUSSIAN;

        [TestMethod()]
        public void ContainsTest()
        {
            
        }

        [TestMethod()]
        public void PositionTest1()
        {
            Assert.AreEqual(1, alphabet.Position('б'));
        }

        [TestMethod()]
        public void PositionTest2()
        {
            Assert.AreEqual(5, alphabet.Position('е'));
        }

        [TestMethod()]
        public void PositionTest3()
        {
            Assert.AreEqual(6, alphabet.Position('ё'));
        }

        [TestMethod()]
        public void PositionTest4()
        {
            Assert.AreEqual(7, alphabet.Position('ж'));
        }
    }
}