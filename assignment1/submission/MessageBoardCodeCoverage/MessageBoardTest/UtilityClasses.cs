using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActorSystem;
using MessageBoard;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimulatedActorUnitTests
{
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

    public class TestDispatcher : SimulatedActor
    {
        SimulatedActorSystem system;
        List<Worker> workers;
        Dispatcher.Mode mode { get; set; }
        TestMessageStore store;
        List<long> acksToCollect;

        public TestDispatcher(SimulatedActorSystem system, int numberOfWorkers)
        {
            this.system = system;
            this.workers = new List<Worker>(numberOfWorkers);
            this.mode = Dispatcher.Mode.NORMAL;
            this.acksToCollect = new List<long>();
            this.channel = new DeterministicChannel(100);
        }

        public override void atStartUp()
        {
            store = new TestMessageStore();
            base.atStartUp();
            for (int i = 0; i < workers.Capacity; i++)
            {
                Worker w = new Worker(this, store, system);
                system.Spawn(w);
                workers.Add(w);
            }
            system.Spawn(store);
        }

        public override void Receive(Message m)
        {
            if (m is InitCommunication)
            {
                // decide upon id for now, maybe switch to login credentials TODO
                InitCommunication initC = ((InitCommunication)m);
                int rnd = new Random((int)initC.CommunicationId).Next();
                int index = (((rnd % workers.Count) + workers.Count) % workers.Count);
                Worker w = workers[index];
                w.Tell(m);
            }
        }
    }

    public class TestMessageStore : MessageStore
    {
        public TestMessageStore()
        {
            this.messages = new Dictionary<long, UserMessage>();
            this.currentId = 0;
            this.channel = new DeterministicChannel(100);
        }
    }
}
