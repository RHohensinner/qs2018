using FsCheck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace FsCheckExample
{
    [TestClass]
    public class FsCheckExampleTest
    {
        /// <summary>
        /// pow implementation
        /// </summary>
        public static int Pow(int x, uint n)
        {
            int y = 1; //result initialization
            while (true)
            {
                if ((n & 1) != 0) y *= x; // if n is odd multiply by x
                n = n >> 1; // position of the next n bit
                if (n == 0) return y; // if there are no more bits return y
                x *= x;  // exponent for the next n bit
            }
        }

        /// <summary>
        /// regular unit test
        /// </summary>
        [TestMethod]
        public void PowTest()
        {
            //standard cases
            Assert.AreEqual(8, Pow(2, 3));
            Assert.AreEqual(9, Pow(3, 2));
            Assert.AreEqual(0, Pow(0, 5));

            //initial condition
            Assert.AreEqual(4, Pow(4, 1));

            //recurrence relation
            Assert.AreEqual(Pow(4, 4), Pow(4, 3) * 4);

            //associativity
            Assert.AreEqual(Pow(2, 2 + 3), Pow(2, 2) * Pow(2, 3));

            //Any number raised by the exponent 0 is 1
            Assert.AreEqual(1, Pow(5, 0));
            Assert.AreEqual(1, Pow(0, 0));
        }

        /// <summary>
        /// shows how properties can be specified
        /// </summary>
        [TestMethod]
        public void PowTestFsCheck()
        {
            Func<int, bool> ZeroExponent = b => Pow(b, 0) == 1;
            Func<int, bool> InitialCondition = b => Pow(b, 1) == b;
            Func<int, uint, bool> RecurrenceRelation = (b, n) => Pow(b, n + 1) == Pow(b, n) * b;
            Func<int, uint, uint, bool> Associativity = (b, n, m) => Pow(b, m + n) == Pow(b, n) * Pow(b, m);

            var config = Configuration.VerboseThrowOnFailure;
            config.Replay = FsCheck.Random.mkStdGen(10); //fixed seed

            Prop.ForAll(ZeroExponent).Check(config); //use fixed seed
            Prop.ForAll(InitialCondition).QuickCheckThrowOnFailure();
            Prop.ForAll(RecurrenceRelation).QuickCheckThrowOnFailure();
            Prop.ForAll(Associativity).QuickCheckThrowOnFailure();
        }

        /// <summary>
        /// shows shrinking with wrong property
        /// </summary>
        [TestMethod]
        public void WrongMultPropertyTest()
        {
            Configuration config = Configuration.VerboseThrowOnFailure;
            config.Replay = FsCheck.Random.mkStdGen(50);
            Func<int, int, bool> WrongMultiplyProperty = (a, b) => a == 0 || b == 0 || a * b >= a;
            Prop.ForAll(WrongMultiplyProperty).Check(config);
        }


        /// <summary>
        /// upper case string property
        /// </summary>
        [TestMethod]
        public void StringGenerationTest()
        {
            Arb.Register<MyArbs>(); //registration of arbitrary (needed to use custom generator and shrinker)
            Func<string, bool> AllUpperCaseLetters = s => Regex.IsMatch(s, "^[A-Z]+$");

            var config = Configuration.VerboseThrowOnFailure;
            config.MaxNbOfTest = 100;

            Stopwatch watch = new Stopwatch();
            watch.Start();
            Prop.ForAll(AllUpperCaseLetters).Check(config);
            Debug.WriteLine("Generator:" + watch.Elapsed.ToString());
        }

        /// <summary>
        /// new string generator with post condition
        /// very slow
        /// it is better to generate correct string without post conditions
        /// </summary>
        [TestMethod]
        public void UpperCaseTest()
        {
            Arb.Register<MyArbs>();
            Func<string, bool> AllUpperCaseLetters = s => Regex.IsMatch(s, "^[A-Z]+$");
            Prop.ForAll(AllUpperCaseLetters).VerboseCheckThrowOnFailure();
        }

    }

}
