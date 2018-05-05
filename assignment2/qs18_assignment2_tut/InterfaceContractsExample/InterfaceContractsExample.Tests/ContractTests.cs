using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterfaceContractsExample
{
    [TestClass]
    public class ContractTests
    {
        [TestMethod]
        public void OriginalSearch()
        {
            IExamples ex = new Examples();

            int[] array = new int[] { 0, 1, 2, 3, 4 };
            ex.search(array, 3);
        }

        [TestMethod]
        public void SearchExpectedPreconditionFail()
        {
            IExamples ex = new Examples();

            int[] array = new int[] { 5, 1, 2, 3, 4 };

            MyAssert.PreConditionFailed(() => ex.search(array, 1));
        }

        [TestMethod]
        public void OriginalSqrt()
        {
            IExamples ex = new Examples();

            double result = ex.sqrt(9);
        }

        [TestMethod]
        public void MutantSqrtExceptionThrownButNotExpected()
        {
            IExamples ex = new Mutant();

            double result = ex.sqrt(9);
        }


    }
}
