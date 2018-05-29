using FsCheck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsCheckExample
{
    [TestClass]
    public class CounterTest
    {
       
        /// <summary>
        /// Model based testing of Counter Specification
        /// </summary>
        [TestMethod]
        public void CounterSpecTest()
        {
            var config = Configuration.VerboseThrowOnFailure;
            new CounterSpec().ToProperty().Check(config);
        }
    }

}
