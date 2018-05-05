// <copyright file="DispatcherTest.cs">Copyright �  2014</copyright>
using System;
using MessageBoard;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ActorSystem;
using System.Linq;

namespace MessageBoard
{

    /// <summary>
    /// Utility class providing access to the members of a Dispatcher
    /// </summary>
    public class DispatcherPublic : Dispatcher
    {
        public Mode ModeProp { get { return this.mode; } set { this.mode = value; } }
        public List<long> AcksToCollect { get { return this.acksToCollect; } set { this.acksToCollect = value; } }
        public List<Worker> Worker { get { return this.workers; } }
        public SimulatedActorSystem System { get { return this.system; } }
        public MessageStore Store { get { return this.messageStore; } }
        public SimulatedActorSystemPublic TestSystem { get; set; }

        public DispatcherPublic(SimulatedActorSystem system, int numberOfWorkers)
        : base(system, numberOfWorkers)
        {

        }
    }
    /// <summary>
    /// Utility class providing access to the actors of a SimulatedActorSystem
    /// </summary>
    public class SimulatedActorSystemPublic : SimulatedActorSystem
    {
        public List<SimulatedActor> Actors { get { return this.actors; } }
    }
    /// <summary>This class contains parameterized unit tests for Dispatcher.
    /// Use <c>[PexAssumeUnderTest]DispatcherPublic target</c> as parameter to test with the 
    /// DispatcherPublic-class.
    /// </summary>
    [PexClass(typeof(Dispatcher))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class DispatcherTest
    {
        [PexMethod]
        public void DummyTest([PexAssumeUnderTest]DispatcherPublic target, [PexAssumeNotNull]InitCommunication m)
        {
            // TODO: assume, exercise, assert
        }
    }
}
