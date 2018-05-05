using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageBoard;
using ActorSystem;
using System.Collections.Generic;
using System.Linq;

namespace SimulatedActorUnitTests
{
    [TestClass]
    public class WorkerTests
    {

        /// <summary>
        /// Simple first test initiating a communication and closing it afterwards.
        /// </summary>
        /// 
        [TestMethod]
        public void TestCommunication()
        {
            //testing only the acks
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);
            // send request and run system until a response is received
            // communication id is chosen by clients 
            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(InitAck), initAckMessage.GetType());
            InitAck initAck = (InitAck)initAckMessage;
            Assert.AreEqual(10, initAck.CommunicationId);

            SimulatedActor worker = initAck.Worker;

            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(FinishAck), finAckMessage.GetType());
            FinishAck finAck = (FinishAck)finAckMessage;

            Assert.AreEqual(10, finAck.CommunicationId);
            dispatcher.Tell(new Stop());
            client.Tell(new Stop());


            // TODO run system until workers and dispatcher are stopped

            //Trail&Error: 18 Tick()s to finish completely
            system.RunFor(18);


        }

        // IMPLEMENTED TESTCASES

        [TestMethod]
        public void TestEverythingAsExpected()
        {

            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(InitAck), initAckMessage.GetType());
            InitAck initAck = (InitAck)initAckMessage;
            Assert.AreEqual(10, initAck.CommunicationId);

            SimulatedActor worker = initAck.Worker;


// 

            // testing Publish
            worker.Tell(new Publish(new UserMessage("richard", "helloworld"), 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationAck), publishMessage.GetType());
            OperationAck publish = (OperationAck)publishMessage;
            Assert.AreEqual(10, publish.CommunicationId);

            // testing publish fail
            UserMessage failingMessage = new UserMessage("ralf", "i will fail for sure, because my length > 10");
            worker.Tell(new Publish(failingMessage, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message publishFailingMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationFailed), publishFailingMessage.GetType());
            OperationFailed publishFailing = (OperationFailed)publishFailingMessage;
            Assert.AreEqual(10, publishFailing.CommunicationId);


            // testing Retrieve
            worker.Tell(new RetrieveMessages("richard", 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message retrieveMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(FoundMessages), retrieveMessage.GetType());
            FoundMessages retrieve = (FoundMessages)retrieveMessage;
            Assert.AreEqual(1, retrieve.Messages.Count);

            //testing Like
            worker.Tell(new Like("everybody", 10, 0));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message likeMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationAck), likeMessage.GetType());
            OperationAck like = (OperationAck)likeMessage;
            Assert.AreEqual(10, like.CommunicationId);


            //testing Dislike
            worker.Tell(new Dislike("nobody", 10, 0));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message dislikeMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationAck), dislikeMessage.GetType());
            OperationAck dislike = (OperationAck)dislikeMessage;
            Assert.AreEqual(10, dislike.CommunicationId);


//

            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(FinishAck), finAckMessage.GetType());
            FinishAck finAck = (FinishAck)finAckMessage;

            Assert.AreEqual(10, finAck.CommunicationId);
            dispatcher.Tell(new Stop());
            client.Tell(new Stop());

            system.RunFor(18);

        }

        [TestMethod]
        public void TestPublish()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // testing Publish
            worker.Tell(new Publish(new UserMessage("richard", "helloworld"), 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationAck), publishMessage.GetType());
            OperationAck publish = (OperationAck)publishMessage;
            Assert.AreEqual(10, publish.CommunicationId);

            // testing publish fail #1
            UserMessage failingMessage = new UserMessage("ralf", "i will fail for sure, because my length > 10");
            failingMessage.MessageId = 0;
            worker.Tell(new Publish(failingMessage, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message publishFailingMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationFailed), publishFailingMessage.GetType());
            OperationFailed publishFailing = (OperationFailed)publishFailingMessage;
            Assert.AreEqual(10, publishFailing.CommunicationId);

            // testing publish fail #2
            UserMessage failingMessage2 = new UserMessage("rudolf", "test #2");
            failingMessage2.MessageId = 0;
            worker.Tell(new Publish(failingMessage2, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message publishFailingMessage2 = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationFailed), publishFailingMessage2.GetType());
            OperationFailed publishFailing2 = (OperationFailed)publishFailingMessage2;
            Assert.AreEqual(10, publishFailing2.CommunicationId);


            // connection fin
            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            FinishAck finAck = (FinishAck)finAckMessage;

            dispatcher.Tell(new Stop());
            client.Tell(new Stop());

            system.RunFor(18);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownClientException), "Unknown communication ID")]
        public void TestPublishException()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // Will throw ExpectedException because communication ID is unkown
            UserMessage exceptionMessage = new UserMessage("rolf", "i am valid");
            worker.Tell(new Publish(exceptionMessage, 9));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
        }

        [TestMethod]
        public void TestRetrieveMessages()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // Reference Publish
            worker.Tell(new Publish(new UserMessage("richard", "helloworld"), 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage = client.ReceivedMessages.Dequeue();
            OperationAck publish = (OperationAck)publishMessage;

            // testing Retrieve
            worker.Tell(new RetrieveMessages("richard", 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message retrieveMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(FoundMessages), retrieveMessage.GetType());
            FoundMessages retrieve = (FoundMessages)retrieveMessage;
            Assert.AreEqual(1, retrieve.Messages.Count);

            // connection fin
            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            FinishAck finAck = (FinishAck)finAckMessage;

            dispatcher.Tell(new Stop());
            client.Tell(new Stop());

            system.RunFor(18);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownClientException), "Unknown communication ID")]
        public void TestRetrieveMessagesException()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // Will throw ExpectedException because communication ID is unkown
            worker.Tell(new RetrieveMessages("richard", 8));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
        }

        [TestMethod]
        public void TestLike()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // Reference Publish
            worker.Tell(new Publish(new UserMessage("richard", "helloworld"), 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage = client.ReceivedMessages.Dequeue();
            OperationAck publish = (OperationAck)publishMessage;

            // Reference Retrieve
            worker.Tell(new RetrieveMessages("richard", 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message retrieveMessage = client.ReceivedMessages.Dequeue();
            FoundMessages retrieve = (FoundMessages)retrieveMessage;

            //testing Like
            worker.Tell(new Like("everybody", 10, 0));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message likeMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationAck), likeMessage.GetType());
            OperationAck like = (OperationAck)likeMessage;
            Assert.AreEqual(10, like.CommunicationId);

            // testing failing Like wrong id
            worker.Tell(new Like("everybody", 10, 1));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message likeFailingMessage1 = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationFailed), likeFailingMessage1.GetType());

            // testing failing Dislike wrong clientName
            worker.Tell(new Like("everybody", 10, 0));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message likeFailingMessage2 = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationFailed), likeFailingMessage2.GetType());

            // connection fin
            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            FinishAck finAck = (FinishAck)finAckMessage;

            dispatcher.Tell(new Stop());
            client.Tell(new Stop());

            system.RunFor(18);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownClientException), "Unknown communication ID")]
        public void TestLikeException()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // Will throw ExpectedException because communication ID is unkown
            worker.Tell(new Like("everybody", 7, 0));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
        }

        [TestMethod]
        public void TestDislike()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // Reference Publish
            worker.Tell(new Publish(new UserMessage("richard", "helloworld"), 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage = client.ReceivedMessages.Dequeue();
            OperationAck publish = (OperationAck)publishMessage;

            // Reference Retrieve
            worker.Tell(new RetrieveMessages("richard", 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message retrieveMessage = client.ReceivedMessages.Dequeue();
            FoundMessages retrieve = (FoundMessages)retrieveMessage;

            //testing Dislike
            worker.Tell(new Dislike("nobody", 10, 0));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message dislikeMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationAck), dislikeMessage.GetType());
            OperationAck like = (OperationAck)dislikeMessage;
            Assert.AreEqual(10, like.CommunicationId);

            // testing failing Dislike wrong id
            worker.Tell(new Dislike("nobody", 10, 1));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message dislikeFailingMessage1 = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationFailed), dislikeFailingMessage1.GetType());

            // testing failing Dislike wrong clientName
            worker.Tell(new Dislike("nobody", 10, 0));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message dislikeFailingMessage2 = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationFailed), dislikeFailingMessage2.GetType());

            // connection fin
            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            FinishAck finAck = (FinishAck)finAckMessage;

            dispatcher.Tell(new Stop());
            client.Tell(new Stop());

            system.RunFor(18);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownClientException))]
        public void TestDislikeException()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // Will throw ExpectedException because communication ID is unkown
            worker.Tell(new Dislike("nobody", 6, 0));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownClientException))]
        public void TestFinishCommunicationException()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // Will throw ExpectedException because communication ID is unkown
            initAck.Worker.Tell(new FinishCommunication(5));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
        }
        [TestMethod]
        public void TestUserMessageToString()
        {
            UserMessage basic = new UserMessage("Hans", "Test");
            String reference = basic.ToString();
            Assert.AreEqual("Hans:Test liked by : disliked by :", reference);
        }

        [TestMethod]
        public void TestRunUntill()
        {
            SimulatedActorSystem system = new SimulatedActorSystem();
            int now = system.currentTime;
            int after = system.currentTime + 6;
            system.RunUntil(system.currentTime + 5);
            Assert.AreEqual(after, system.currentTime);
        }

        [TestMethod]
        public void TestUpdateWithPublish()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // Publish
            UserMessage duplicate = new UserMessage("richard", "HEY");
            UserMessage sameAuthor = new UserMessage("richard", "HEJ");
            UserMessage sameMessage = new UserMessage("reinhardt", "HEY");

            worker.Tell(new Publish(new UserMessage("richard", "HEY"), 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage1 = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationAck), publishMessage1.GetType());
            OperationAck publish1 = (OperationAck)publishMessage1;
            Assert.AreEqual(10, publish1.CommunicationId);

            worker.Tell(new Publish(duplicate, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage2 = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationFailed), publishMessage2.GetType());
            OperationFailed publish2 = (OperationFailed)publishMessage2;
            Assert.AreEqual(10, publish2.CommunicationId);

            worker.Tell(new Publish(sameAuthor, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage3 = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationAck), publishMessage3.GetType());
            OperationAck publish3 = (OperationAck)publishMessage3;
            Assert.AreEqual(10, publish3.CommunicationId);

            worker.Tell(new Publish(sameMessage, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage4 = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationAck), publishMessage4.GetType());
            OperationAck publish4 = (OperationAck)publishMessage4;
            Assert.AreEqual(10, publish4.CommunicationId);

            // connection fin
            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            FinishAck finAck = (FinishAck)finAckMessage;

            dispatcher.Tell(new Stop());
            client.Tell(new Stop());

            system.RunFor(18);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownClientException))]
        public void TestClientMessageWhileStopping()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // connection fin
            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            FinishAck finAck = (FinishAck)finAckMessage;

            dispatcher.Tell(new Stop());
            client.Tell(new Stop());

            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);


            Message stopMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(Stop), stopMessage.GetType());
            Stop stop = (Stop)stopMessage;

            worker.Tell(new Publish(new UserMessage("richard", "helloworld"), 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
        }

        [TestMethod]
        public void TestInitCommunicationWhileStop()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);
            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(InitAck), initAckMessage.GetType());
            InitAck initAck = (InitAck)initAckMessage;
            Assert.AreEqual(10, initAck.CommunicationId);

            SimulatedActor worker = initAck.Worker;

            // connection fin
            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(FinishAck), finAckMessage.GetType());
            FinishAck finAck = (FinishAck)finAckMessage;

            Assert.AreEqual(10, finAck.CommunicationId);
            dispatcher.Tell(new Stop());
            client.Tell(new Stop());


            // dispatcher check if init fails while Stop(), should fail
            dispatcher.Tell(new InitCommunication(client, 11));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message initCommFailMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(Stop), initCommFailMessage.GetType());

            system.RunFor(18);

        }

        [TestMethod]
        public void TestRetries()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            Dispatcher dispatcher = new Dispatcher(system, 20);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // testing Publish
            worker.Tell(new Publish(new UserMessage("richard", "helloworld"), 10));
            dispatcher.Tell(new Stop());

            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationAck), publishMessage.GetType());
            OperationAck publish = (OperationAck)publishMessage;
            Assert.AreEqual(10, publish.CommunicationId);


            // connection fin
            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationFailed), finAckMessage.GetType());
            OperationFailed finAck = (OperationFailed)finAckMessage;
            Assert.AreEqual(10, finAck.CommunicationId);

            client.Tell(new Stop());

            system.RunFor(18);
        }

        [TestMethod]
        public void TestMessageDrop()
        {
            //connection init
            SimulatedActorSystem system = new SimulatedActorSystem();
            TestDispatcher dispatcher = new TestDispatcher(system, 2);
            system.Spawn(dispatcher);
            TestClient client = new TestClient();
            system.Spawn(client);

            dispatcher.Tell(new InitCommunication(client, 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);
            Message initAckMessage = client.ReceivedMessages.Dequeue();
            InitAck initAck = (InitAck)initAckMessage;

            SimulatedActor worker = initAck.Worker;

            // testing Publish
            worker.Tell(new Publish(new UserMessage("edith", "hellothere"), 10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message publishMessage = client.ReceivedMessages.Dequeue();
            Assert.AreEqual(typeof(OperationFailed), publishMessage.GetType());
            OperationFailed publish = (OperationFailed)publishMessage;
            Assert.AreEqual(10, publish.CommunicationId);


            // connection fin
            initAck.Worker.Tell(new FinishCommunication(10));
            while (client.ReceivedMessages.Count == 0)
                system.RunFor(1);

            Message finAckMessage = client.ReceivedMessages.Dequeue();
            FinishAck finAck = (FinishAck)finAckMessage;

            dispatcher.Tell(new Stop());
            client.Tell(new Stop());

            system.RunFor(18);
        }

    }
}
