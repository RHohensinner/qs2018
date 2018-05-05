using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SocialActivity
{
    [TestClass]
    public class SocialActivityFunctionTest
    {

        [ClassCleanup]
        public static void tearDown()
        {
            ArgumentLogger.createLogFile();
        }

        [TestMethod]
        public void Case1()
        {
            double case1 = SocialActivityFunction.getSocialActivityFactor(10, 100, 0);
            Assert.AreEqual(case1, 15.0);
        }

        [TestMethod]
        public void Case2()
        {
            double case2 = SocialActivityFunction.getSocialActivityFactor(10, 100, 1);
            Assert.AreEqual(case2, 19.625);
        }

        [TestMethod]
        public void Case3()
        {
            double case3 = SocialActivityFunction.getSocialActivityFactor(10, 100, 6);
            Assert.AreEqual(case3, 42.75);
        }

        [TestMethod]
        public void Case4()
        {
            double case4 = SocialActivityFunction.getSocialActivityFactor(10, 100, 12);
            Assert.AreEqual(case4, 70.5);
        }

        [TestMethod]
        public void Case5()
        {
            double case5 = SocialActivityFunction.getSocialActivityFactor(10, 100, 13);
            Assert.AreEqual(case5, 75.125);
        }

        [TestMethod]
        public void Case5a()
        {
            double case5a = SocialActivityFunction.getSocialActivityFactor(10, 100, 14);
            Assert.AreEqual(case5a, -1);
        }

        [TestMethod]
        public void Case5b()
        {
            double case5b = SocialActivityFunction.getSocialActivityFactor(10, 100, -1);
            Assert.AreEqual(case5b, -1);
        }


        [TestMethod]
        public void Case6()
        {
            double case6 = SocialActivityFunction.getSocialActivityFactor(10, 0, 6);
            Assert.AreEqual(case6, 27.75);
        }

        [TestMethod]
        public void Case7()
        {
            double case7 = SocialActivityFunction.getSocialActivityFactor(10, 1, 6);
            Assert.AreEqual(case7, 27.9);
        }

        [TestMethod]
        public void Case8()
        {
            double case8 = SocialActivityFunction.getSocialActivityFactor(10, 199, 6);
            Assert.AreEqual(case8, 57.6);
        }

        [TestMethod]
        public void Case9()
        {
            double case9 = SocialActivityFunction.getSocialActivityFactor(10, 200, 6);
            Assert.AreEqual(case9, 57.75);
        }

        [TestMethod]
        public void Case9a()
        {
            double case9a = SocialActivityFunction.getSocialActivityFactor(10, 201, 6);
            Assert.AreEqual(case9a, -1);
        }

        [TestMethod]
        public void Case9b()
        {
            double case9b = SocialActivityFunction.getSocialActivityFactor(10, -1, 6);
            Assert.AreEqual(case9b, -1);
        }

        [TestMethod]
        public void Case10()
        {
            double case10 = SocialActivityFunction.getSocialActivityFactor(0, 100, 6);
            Assert.AreEqual(case10, 37.75);
        }

        [TestMethod]
        public void Case11()
        {
            double case11 = SocialActivityFunction.getSocialActivityFactor(1, 100, 6);
            Assert.AreEqual(case11, 38.25);
        }

        [TestMethod]
        public void Case12()
        {
            double case12 = SocialActivityFunction.getSocialActivityFactor(19, 100, 6);
            Assert.AreEqual(case12, 47.25);
        }

        [TestMethod]
        public void Case13()
        {
            double case13 = SocialActivityFunction.getSocialActivityFactor(20, 100, 6);
            Assert.IsTrue(case13 > 0 && case13 < 100);

        }

        [TestMethod]
        public void Case13a()
        {
            double case13a = SocialActivityFunction.getSocialActivityFactor(21, 100, 6);
            Assert.AreEqual(case13a, -1);
        }

        [TestMethod]
        public void Case13b()
        {
            double case13b = SocialActivityFunction.getSocialActivityFactor(-1, 100, 6);
            Assert.AreEqual(case13b, -1);
        }

    }
}