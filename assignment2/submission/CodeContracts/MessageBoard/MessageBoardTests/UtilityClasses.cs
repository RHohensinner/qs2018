using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActorSystem;
using MessageBoard;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.Contracts;

namespace SimulatedActorUnitTests
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
   
    /// <summary>
    /// Simple actor, which can be used in tests, e.g. to check if
    /// the correct messages are sent by workers. This actor can be sent 
    /// to workers as client.
    /// </summary>
    public class TestClient : SimulatedActor
    {
        /// <summary>
        /// messages received by this actor.
        /// </summary>
        public Queue<Message> ReceivedMessages { get; private set; }
        public TestClient()
        {
            ReceivedMessages = new Queue<Message>();
        }
        /// <summary>
        /// does not implement any logic, only saves the received messages
        /// </summary>
        /// <param name="m"></param>
        public override void Receive(Message m)
        {
            ReceivedMessages.Enqueue(m);
        }

    }

    /// <summary>
    /// Utility class for parameterized unit testing, which offers to
    /// set the duration using the constructor. This way, it is possible
    /// to write parameterized unit tests, which take integers or 
    /// integers arrays as parameter, which can easily be transformed to 
    /// test messages.
    /// </summary>
    public class PexTestMessage : Message
    {
        private int duration;
        public PexTestMessage(int duration)
        {
            this.duration = duration;
        }
        public int Duration()
        {
            return this.duration;
        }
    }

    public class TestSimulatedActor : SimulatedActor
    {
        public Message ReceivedMessage { get; set; }

        public override void Receive(Message m)
        {
            ReceivedMessage = m;
        }
    }
}
