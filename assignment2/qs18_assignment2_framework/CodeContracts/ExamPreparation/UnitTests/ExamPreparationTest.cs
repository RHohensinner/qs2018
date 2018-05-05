using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExamPreparation;

namespace UnitTests
{
    [TestClass]
    public class ExamPreparationTest
    {
        [TestMethod]
        public void SortTest()
        {
            IExamPreparation prep = new ExamPreparation.ExamPreparation();

            int[] arr = new int[] { 1, 3, 2 };
            int[] res = prep.SortDescending(arr);

            Assert.AreEqual(arr.Length, res.Length);
            Assert.AreEqual<int[]>(new int[] { 3, 2, 1 }, res);
        }

        //TODO: It is a good idea to write more tests, to ensure the Contracts work as intended
    }
}
