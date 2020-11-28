using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrypterCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CrypterCoreTests
{
    [TestClass]
    public class CyclicArrayTests
    {
        [TestMethod]
        public void Test1()
        {
            var ar = new CyclicArray<int> (new []{0, 1, 2, 3, 4});
            Assert.AreEqual(0, ar[5] );
        }

        [TestMethod]
        public void Test2()
        {
            var ar = new CyclicArray<int>(new[] { 0, 1, 2, 3, 4 });
            Assert.AreEqual(4, ar[-1]);
        }

        [TestMethod]
        public void Test3()
        {
            var ar = new CyclicArray<int>(new[] { 0, 1, 2, 3, 4 });
            Assert.AreEqual(0, ar[10]);
        }

        [TestMethod]
        public void Test4()
        {
            var ar = new CyclicArray<int>(new[] { 0, 1, 2, 3, 4 });
            Assert.AreEqual(4, ar[-6]);
        }
    }
}
