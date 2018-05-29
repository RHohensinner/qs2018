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
        public void YourTest()
        {
            var examPrep = new ExamPreparation.ExamPreparation();

            var intersection = examPrep.Intersect(new int[] { 1, 2, 3 }, new int[] { 2, 3, 4 });
            CollectionAssert.AreEqual(intersection, new int[] { 2, 3 });

            var isProperSubset = examPrep.IsProperSubsetOf(new int[] { 1, 2 }, new int[] { 1, 2, 3 });
            Assert.IsTrue(isProperSubset);
        }

        //TODO: It is a good idea to write tests, to ensure the Contracts work as intended
    }
}
