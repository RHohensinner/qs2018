using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Contracts;
using SimulatedActorUnitTests;

namespace ActorSystem
{
    /// <summary>
    /// Summary description for SimulatedActorSystemContractTests
    /// </summary>
    [TestClass]
    public class SimulatedActorSystemContractTests
    {
        public SimulatedActorSystemContractTests()
        {
        }

        //TODO: It is a good idea to write tests, to ensure the Contracts work as intended
        [TestMethod]
        public void ISAS1_optimisticCurrentTime()
        {
            ISimulatedActorSystem system = new SimulatedActorSystem();

            system.RunFor(1);
            Assert.AreEqual(system.currentTime, 1);

            MyAssert.PreConditionFailed(() => system.RunFor(-1));
            MyAssert.PreConditionFailed(() => system.RunFor(0));

            system.RunUntil(10);
            Assert.AreEqual(system.currentTime, 11);

            MyAssert.PreConditionFailed(() => system.RunUntil(-1));
            MyAssert.PreConditionFailed(() => system.RunUntil(0));

            // integer overflow at 2147483647 (0x7FFFFFFF)
            int fromoutofthesewerscomes = unchecked(2147483647 + 1);
            MyAssert.PreConditionFailed(() => system.RunUntil(fromoutofthesewerscomes));
            MyAssert.PreConditionFailed(() => system.RunFor(fromoutofthesewerscomes));

            // LAZY-MUTANT ----------------------------------------------------------------------------

            ISimulatedActorSystem system2 = new LazyMutant();

            MyAssert.PostConditionOrInvariantFailed(() => system2.RunFor(1));
            Assert.AreEqual(system2.currentTime, 0);

            MyAssert.PreConditionFailed(() => system.RunFor(-1));
            MyAssert.PreConditionFailed(() => system.RunFor(0));

            MyAssert.PostConditionOrInvariantFailed(() => system2.RunUntil(10));
            Assert.AreEqual(system2.currentTime, 0);

            MyAssert.PreConditionFailed(() => system.RunUntil(-1));
            MyAssert.PreConditionFailed(() => system.RunUntil(0));

            // integer overflow at 2147483647 (0x7FFFFFFF)
            int fromoutofthesewerscomes2 = unchecked(2147483647 + 1);
            MyAssert.PreConditionFailed(() => system2.RunUntil(fromoutofthesewerscomes2));
            MyAssert.PreConditionFailed(() => system2.RunFor(fromoutofthesewerscomes2));

        }

        [TestMethod]
        public void ISAS2_actorIdAlwaysPositive()
        {
            ISimulatedActorSystem system = new SimulatedActorSystem();
            TestClient client = new TestClient();
            system.Spawn(client);
            Assert.AreEqual(client.Id, 0);

            TestClient client2 = new TestClient();
            client2.Id = 5;
            system.Spawn(client2);
            Assert.AreEqual(client2.Id, 1);

            // LAZY-MUTANT ----------------------------------------------------------------------------

            ISimulatedActorSystem system2 = new LazyMutant();
            TestClient client3 = new TestClient();
            MyAssert.PostConditionOrInvariantFailed(() => system2.Spawn(client3));
            

            TestClient client4 = new TestClient();
            MyAssert.PostConditionOrInvariantFailed(() => system2.Spawn(client4));


        }

        [TestMethod]
        public void ISAS3_initNoActorsPresent()
        {
            ISimulatedActorSystem system1 = new SimulatedActorSystem();
            ISimulatedActorSystem system2 = new SimulatedActorSystem();
            TestClient client = new TestClient();
            system1.Spawn(client);
            Assert.AreNotEqual(system1, system2);

            // LAZY-MUTANT ----------------------------------------------------------------------------
            // TODO: init actors >= 0 how!?

            ISimulatedActorSystem mutantsystem = new LazyMutant();
            Assert.AreEqual(mutantsystem.currentTime, 0);
        }

        [TestMethod]
        public void ISAS4_initTimeEqualsZero()
        {
            ISimulatedActorSystem system = new SimulatedActorSystem();
            Assert.AreEqual(system.currentTime, 0);

            // LAZY-MUTANT ----------------------------------------------------------------------------
            // TODO: init currentTime >= 0 how!?

            ISimulatedActorSystem system2 = new LazyMutant();
            Assert.AreEqual(system2.currentTime, 0);
        }

        [TestMethod]
        public void ISAS5_noDuplicateActorSpawns()
        {
            ISimulatedActorSystem system = new SimulatedActorSystem();
            TestClient client = new TestClient();
            system.Spawn(client);
            Assert.AreEqual(client.Id, 0);
            TestClient client2 = new TestClient();
            system.Spawn(client2);
            Assert.AreEqual(client2.Id, 1);
            TestClient doom_client = new TestClient();
            system.Spawn(doom_client);

            MyAssert.PreConditionFailed(() => system.Spawn(doom_client));

        }

        [TestMethod]
        public void ISAS6_atSpawnSetTimeSinceSystemStart()
        {
            ISimulatedActorSystem system = new SimulatedActorSystem();
            system.RunFor(2);
            TestClient client = new TestClient();
            system.Spawn(client);
            Assert.AreEqual(client.TimeSinceSystemStart, 2);

            // LAZY-MUTANT ----------------------------------------------------------------------------

            ISimulatedActorSystem system2 = new LazyMutant();
            TestClient client2 = new TestClient();
            MyAssert.PostConditionOrInvariantFailed(() => system2.RunFor(2));
            MyAssert.PostConditionOrInvariantFailed(() => system2.Spawn(client2));
            Assert.AreEqual(client2.TimeSinceSystemStart, -1);
        }

        [TestMethod]
        public void ISAS7_passedTimeNotifiedToActors()
        {
            ISimulatedActorSystem system = new SimulatedActorSystem();
            TestClient client = new TestClient();
            system.Spawn(client);
            Assert.AreEqual(client.TimeSinceSystemStart, 0);

            // LAZY-MUTANT ----------------------------------------------------------------------------

            ISimulatedActorSystem system2 = new LazyMutant();
            TestClient client2 = new TestClient();
            MyAssert.PostConditionOrInvariantFailed(() => system2.Spawn(client2));
            Assert.AreEqual(client2.TimeSinceSystemStart, -1);
        }

        [TestMethod]
        public void ISAS8_runForNotZero()
        {
            ISimulatedActorSystem system = new SimulatedActorSystem();

            system.RunFor(1);
            Assert.AreEqual(system.currentTime, 1);

            MyAssert.PreConditionFailed(() => system.RunFor(0));

            // LAZY-MUTANT ----------------------------------------------------------------------------

            ISimulatedActorSystem system2 = new LazyMutant();

            Assert.AreEqual(system2.currentTime, 0);
            
            MyAssert.PreConditionFailed(() => system2.RunFor(0));
        }

        [TestMethod]
        public void ISAS9_runForGivenTime()
        {
            ISimulatedActorSystem system = new SimulatedActorSystem();

            system.RunFor(20);
            Assert.AreEqual(system.currentTime, 20);

            // LAZY-MUTANT ----------------------------------------------------------------------------

            ISimulatedActorSystem system2 = new LazyMutant();

            MyAssert.PostConditionOrInvariantFailed(() => system2.RunFor(20));
            Assert.AreEqual(system2.currentTime, 0);
        }

        [TestMethod]
        public void ISAS10_runUntilGreaterThanCurrentTime()
        {
            ISimulatedActorSystem system = new SimulatedActorSystem();

            system.RunUntil(10);
            Assert.AreEqual(system.currentTime, 11);

            MyAssert.PreConditionFailed(() => system.RunUntil(10));
        }

        [TestMethod]
        public void ISAS11_runUntilPlusOneEqualsCurrentTime()
        {
            ISimulatedActorSystem system = new SimulatedActorSystem();

            system.RunUntil(20);
            Assert.AreEqual(system.currentTime, 21);

            // LAZY-MUTANT ----------------------------------------------------------------------------

            ISimulatedActorSystem system2 = new LazyMutant();

            MyAssert.PostConditionOrInvariantFailed(() => system2.RunUntil(20));
            Assert.AreEqual(system2.currentTime, 0);
        }
    }
}
