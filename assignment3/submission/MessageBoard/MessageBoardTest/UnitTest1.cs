using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FsCheck;

namespace MessageBoard
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var config = Configuration.VerboseThrowOnFailure;
            config.MaxNbOfTest = 2000;
            new MessageBoardSpecification().ToProperty().Check(config);
        }
    }
}
