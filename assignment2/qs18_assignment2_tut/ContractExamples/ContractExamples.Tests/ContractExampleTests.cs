using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContractExamples
{
    [TestClass]
    public class ContractExampleTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            byte[] pin = new byte[] { 1, 3, 3, 7 };
            Purse p = new Purse(1000, 100, pin);
        }
    }
}
