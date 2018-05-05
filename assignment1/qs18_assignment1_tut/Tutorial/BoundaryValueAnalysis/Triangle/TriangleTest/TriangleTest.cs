using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QS18
{
    [TestClass]
    public class TriangleTest
    {

        [ClassInitialize]
        public static void setup(TestContext context)
        {
            // Initialize stuff; Called before any of the tests
        }

        [ClassCleanup]
        public static void tearDown()
        {
            // Clean up stuff; Called after all the tests
            ArgumentLogger.createLogFile();
        }

        [TestInitialize]
        public void TestInitialize() {
            // Called before a test
        }

        [TestCleanup]
        public void TestCleanup() {
            // Called after a test
        }

        [TestMethod]
        public void Middle()
        {
            string type = Shape.Triangle(10, 10, 10);
            Assert.IsTrue(type.CompareTo("equilateral") == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void OMaxA()
        {
            string type = Shape.Triangle(201, 10, 10);
            // Assert.IsTrue(type.CompareTo("something") != 0);
            // This part is not reached due to the thrown Exception.
        }

        [TestMethod]
        public void MaxA()
        {
            string type = Shape.Triangle(200, 10, 10);
            Assert.IsTrue(type.CompareTo("no triangle") == 0);
        }

        [TestMethod]
        public void UMaxA()
        {
            string type = Shape.Triangle(199, 10, 10);
            Assert.IsTrue(type.CompareTo("no triangle") == 0);
        }

        // TODO: Add more tests


    }
}
