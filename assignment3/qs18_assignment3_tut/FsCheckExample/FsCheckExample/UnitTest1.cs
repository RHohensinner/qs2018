using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FsCheck;

namespace FsCheckExample
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var config = new Configuration();
            config.QuietOnSuccess = true;
            new CounterSpec().ToProperty().VerboseCheckThrowOnFailure();

        }
    }
}
