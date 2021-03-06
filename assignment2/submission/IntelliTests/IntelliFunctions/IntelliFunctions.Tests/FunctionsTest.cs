// <copyright file="FunctionsTest.cs">Copyright ©  2018</copyright>

using System;
using IntelliFunctions;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;


namespace IntelliFunctions.Tests
{
    /// <summary>This class contains parameterized unit tests for Functions</summary>
    [TestClass]
    [PexClass(typeof(Functions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class FunctionsTest
    {
        /// <summary>Test stub for SortDescending(Int32[])</summary>
        [PexMethod(MaxRunsWithoutNewTests = 500)]
        public int[] SortDescendingTest(int[] array)
        {
            // TODO: add assertions to method FunctionsTest.SortDescendingTest(Int32[])

            PexAssume.IsNotNull(array);

            int[] result = Functions.SortDescending(array);

            int[] expected = new int[array.Length];

            array.CopyTo(expected, 0);

            Array.Sort(expected);
            Array.Reverse(expected);

            PexAssert.IsTrue(expected.Length == result.Length);
            PexAssert.IsTrue(result.All(x => expected.Contains(x)) && expected.All(x => result.Contains(x)));

            return result;
        }

        /// <summary>Test stub for IsSubsetOf(Int32[], Int32[])</summary>
        [PexMethod]
        public bool IsSubsetOfTest(int[] subset, int[] superset)
        {
            PexAssume.IsNotNull(subset);
            PexAssume.IsNotNull(superset);
            PexAssume.AreEqual(subset.Count(), subset.Distinct().Count());
            PexAssume.AreEqual(superset.Count(), superset.Distinct().Count());

            bool result = Functions.IsSubsetOf(subset, superset);
            bool expected = subset.All(x => superset.Contains(x));

            PexAssert.AreEqual(result, expected);

            return result;
            // TODO: add assertions to method FunctionsTest.IsSubsetOfTest(Int32[], Int32[])
        }

        /// <summary>Test stub for CombinationWithoutRepetition(Int32[], Int32)</summary>
        [PexMethod]
        public int[][] CombinationWithoutRepetitionTest(int[] set, int numberOfElements)
        {
            PexAssume.IsNotNull(set);
            PexAssume.IsNotNull(numberOfElements);
            PexAssume.IsTrue(numberOfElements >= 1 && numberOfElements <= set.Length);
            PexAssume.AreEqual(set.Count(), set.Distinct().Count());
            PexAssume.IsTrue(set.Count() <= 10);
            PexAssume.IsTrue(numberOfElements <= 10);

            int[][] result = Functions.CombinationWithoutRepetition(set, numberOfElements);

            PexAssert.IsTrue(result.All(x => x.Count().Equals(numberOfElements)));
            PexAssert.IsTrue(result.All(x => x.Distinct().Count().Equals(x.Count())));

            return result;
            // TODO: add assertions to method FunctionsTest.CombinationWithoutRepetitionTest(Int32[], Int32)
        }

    }
}
