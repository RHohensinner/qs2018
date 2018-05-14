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
            CollectionAssert.AreEqual(new int[] { 3, 2, 1 }, res);
        }

        [TestMethod]
        public void sortDescendingTest()
        {
            IExamPreparation prep = new ExamPreparation.ExamPreparation();

            int[] arr = new int[] { 1, 3, 2 };
            int[] res = prep.SortDescending(arr);

            Assert.AreEqual(arr.Length, res.Length);
            CollectionAssert.AreEqual(new int[] { 3, 2, 1 }, res);
        }

        [TestMethod]
        public void minTest()
        {
            IExamPreparation prep = new ExamPreparation.ExamPreparation();

            int[] arr = new int[] { 1, 3, 2 };
            int res = prep.Min(arr);

            Assert.AreEqual(1, res);

            int[] arr2 = new int[0];
            MyAssert.PreConditionFailed(() => prep.Min(arr2));
        }

        [TestMethod]
        public void maxIndexTest()
        {
            IExamPreparation prep = new ExamPreparation.ExamPreparation();

            int[] arr = new int[] { 1, 3, 2 };
            int res = prep.MaxIndex(arr);

            Assert.AreEqual(1, res);

            int[] arr2 = new int[0];
            MyAssert.PreConditionFailed(() => prep.MaxIndex(arr2));
        }

        [TestMethod]
        public void symmetricDifferencesTest()
        {
            IExamPreparation prep = new ExamPreparation.ExamPreparation();

            int[] arr1 = new int[] { 1, 3, 2 };
            int[] arr2 = new int[] { 5, 3, 4 };
            int[] res = prep.SymmetricDifference(arr1, arr2);

            Assert.AreEqual(4, res.Length);
        }

        [TestMethod]
        public void relationXIsBiggerThanY()
        {
            IExamPreparation prep = new ExamPreparation.ExamPreparation();

            int[] arr = new int[] { 1, 3, 2 };
            Tuple<int, int>[] res = prep.RelationXIsBiggerThanY(arr);

            Assert.AreEqual(3, res.Length);
        }

        //TODO: It is a good idea to write more tests, to ensure the Contracts work as intended
    }
}
