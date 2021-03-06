using Microsoft.Pex.Framework.Exceptions;
// <copyright file="FunctionsTest.cs">Copyright ©  2016</copyright>
using System;
using IntelliFunctions;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace IntelliFunctions.Tests
{
    /// <summary>This class contains parameterized unit tests for Functions</summary>
    [PexClass(typeof(Functions))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class FunctionsTest
    {


        /// <summary>Test stub for Union(Int32[], Int32[])</summary>
        [PexMethod]
        public int[] UnionTest(int[] a1, int[] a2)
        {
            PexAssume.IsNotNull(a1);
            PexAssume.IsNotNull(a2);

            PexAssume.IsTrue(a1.Distinct().Count() == a1.Count());
            PexAssume.IsTrue(a2.Distinct().Count() == a2.Count());

            int[] result = Functions.Union(a1, a2);

            PexAssert.IsTrue(result.Distinct().Count() == result.Count());

            int[] resultExpected = a1.Union(a2).ToArray();

            PexAssert.AreEqual(result.Length, resultExpected.Length);

            PexAssert.IsTrue(result.All(x => resultExpected.Contains(x)) && resultExpected.All(x => result.Contains(x)));

            return result;
        }
    }
}
