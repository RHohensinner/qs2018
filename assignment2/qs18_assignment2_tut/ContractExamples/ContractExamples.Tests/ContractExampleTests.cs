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
            //byte[] qin = new byte[] { 1, 2, 3 };
            Purse p = new Purse(1000, 100, pin);
            //Purse q = new Purse(40, 30, qin);
            //Purse r = new Purse(-10, -5, pin);

            p.debit(100);
            p.checkPin(pin);
        }
    }
}
