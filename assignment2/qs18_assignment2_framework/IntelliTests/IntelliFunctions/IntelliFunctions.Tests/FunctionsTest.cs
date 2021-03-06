// <copyright file="FunctionsTest.cs">Copyright ©  2018</copyright>

using System;
using IntelliFunctions;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        [PexMethod]
        public int[] SortDescendingTest(int[] array)
        {
            int[] result = Functions.SortDescending(array);
            return result;
            // TODO: add assertions to method FunctionsTest.SortDescendingTest(Int32[])
        }

        /// <summary>Test stub for IsSubsetOf(Int32[], Int32[])</summary>
        [PexMethod]
        public bool IsSubsetOfTest(int[] subset, int[] superset)
        {
            bool result = Functions.IsSubsetOf(subset, superset);
            return result;
            // TODO: add assertions to method FunctionsTest.IsSubsetOfTest(Int32[], Int32[])
        }

        /// <summary>Test stub for CombinationWithoutRepetition(Int32[], Int32)</summary>
        [PexMethod]
        public int[][] CombinationWithoutRepetitionTest(int[] set, int numberOfElements)
        {
            int[][] result = Functions.CombinationWithoutRepetition(set, numberOfElements);
            return result;
            // TODO: add assertions to method FunctionsTest.CombinationWithoutRepetitionTest(Int32[], Int32)
        }

    }
}
