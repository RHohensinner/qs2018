using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Contracts;

namespace ExamPreparation
{
    /// <summary>
    /// Utility class containing an assertion-method for exceptions
    /// </summary>
    public static class MyAssert
    {
        /// <summary>
        /// Tests if a given action throws an exception and if the exception is
        /// actually the exception, we expected. Use e.g. as in:
        /// <c>
        ///    int x = 0;
        ///    MyAssert.Throws(
        ///        () => Console.WriteLine(1 / x), 
        ///        e => e.GetType().Equals(typeof(DivideByZeroException))); 
        /// </c>
        /// </summary>
        /// <param name="testAction">an anonymous function throwing some exception</param>
        /// <param name="exceptionVerifier">a predicate, which checks 
        /// if the exception is the excepted exception</param>
        public static void Throws(Action testAction, Predicate<Exception> exceptionVerifier)
        {
            try
            {
                testAction();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().ToString());
                if (exceptionVerifier(e))
                    return;
                Assert.Fail("Exception \"" + e.Message + "\" did not match expectations");
            }
            Assert.Fail("No exception was thrown at all");
        }
        /// <summary>
        /// Helper method to check if contracts work. It can e.g. be used in tests,
        /// which test if the contracts correctly identify mutants. Since contract
        /// exceptions are private types we need to check if either postcondition
        /// or invariant specification detected the error.
        /// </summary>
        /// <param name="testAction">Action which should throw a contract exception</param>
        public static void PostConditionOrInvariantFailed(Action testAction)
        {
            Throws(testAction, e => e.Message.Contains("Postcondition failed") || e.Message.Contains("Invariant failed") || e.Message.Contains("Nachbedingungsfehler") || e.Message.Contains("Invariant-Fehler"));
        }
        /// <summary>
        /// Helper method to check if contracts work. It can e.g. be used in tests
        /// to check if contracts correctly identify disallowed inputs as invalid. 
        /// Since contract exceptions are private types we need to check the 
        /// message of the exception.
        /// </summary>
        /// <param name="testAction">Action which should throw a contract exception</param>
        public static void PreConditionFailed(Action testAction)
        {
            Throws(testAction, e => e.Message.Contains("Precondition failed") || e.Message.Contains("Vorbedingungsfehler"));
        }
        
    }

    public static class TestUtilities
    {
        public static void ListEquals<T>(List<T> expected, List<T> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

    }
}
