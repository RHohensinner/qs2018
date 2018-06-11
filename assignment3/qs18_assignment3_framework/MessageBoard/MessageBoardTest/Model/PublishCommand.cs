using ActorSystem;
using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard
{
    public partial class MessageBoardSpecification : ICommandGenerator<MessageBoard, AbstractUserMessageList>
    {

        public class PublishCommand : Command<MessageBoard, AbstractUserMessageList>
        {
            string author;
            string message;
            // string to contain potential specification errors ; will be empty if nothing is found
            string error;

            public PublishCommand(string a, string m)
            {
                author = a;
                message = m;
                // added specification error string
                error = "";
            }

            public override MessageBoard RunActual(MessageBoard a)
            {
                // init system dispatcher and client
                SimulatedActorSystem sa_system = a.System;
                TestClient test_client = a.Client;
                Dispatcher dispatcher = a.Dispatcher;

                // making sure no messages are stored on client beforehand
                if(test_client.ReceivedMessages.Count != 0)
                {
                    test_client.ReceivedMessages.Clear();
                }

                // establish communication as known from prev. assignments

                // helper variables
                InitCommunication con_init = new InitCommunication(test_client, 1);
                FinishCommunication con_fin = new FinishCommunication(1);

                dispatcher.Tell(con_init);

                while(test_client.ReceivedMessages.Count == 0)
                {
                    sa_system.RunFor(1);
                }

                // init worker
                InitAck initAck = (InitAck)test_client.ReceivedMessages.Dequeue();
                SimulatedActor worker = initAck.Worker;

                // create publish and send
                Publish publish = new Publish(new UserMessage(author, message), 1);

                worker.Tell(publish);

                while(test_client.ReceivedMessages.Count == 0)
                {
                    sa_system.RunFor(1);
                }

                // finish communication as known from prev. assignments
                worker.Tell(con_fin);

                while(test_client.ReceivedMessages.Count == 1)
                {
                    sa_system.RunFor(1);
                }

                return a;
                
            }


            public override AbstractUserMessageList RunModel(AbstractUserMessageList m)
            {
                bool R1 = R1CheckLength(message);
                bool R2 = R2CheckNotExisting(author, message, m);

                // check if any requirements are hurt
                if((R1 == false))
                {
                    // atleast one requirement is hurt set Ack to false and return model
                    m.HasReceivedAck = false;
                    return m;
                }

                else if((R2 == false))
                {
                    m.HasReceivedAck = false;
                    return m;
                }

                // no requirements were hurt

                // create UserMessage and set author and message
                AbstractUserMessage au_message = new AbstractUserMessage();
                au_message.Author = author;
                au_message.Message = message;

                // add message to the model, set Ack true and return
                m.Add(au_message);
                m.HasReceivedAck = true;

                return m;
            }

            public override Property Post(MessageBoard a, AbstractUserMessageList m)
            {
                // validator is used to keep track of potentially hurt requirements

                bool validator = true;

                Message msg = a.Client.ReceivedMessages.Dequeue();

                bool R6 = R6CheckOpAck(msg.GetType(), m.HasReceivedAck);
                bool R7 = R7CheckOpFail(msg.GetType(), m.HasReceivedAck);

                bool RX = R6 && R7;

                // splitting if-cases to increase readability
                if(RX == false)
                {
                    validator = false;
                }

                m.HasReceivedAck = false;

                // returning true/false as property
                Property valid_property = validator.ToProperty();
                return valid_property;
            }

            public override string ToString()
            {
                return "Publish(" + author + "," + message + ")" + " " + error;
            }

            // ---------------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------------
            // HELPER FUNCTIONS TO CHECK IF ANY REQUIREMENTS ARE HURT:

            // check wheather R1: message length <= 10, is true or false
            public bool R1CheckLength(string message)
            {
                if(message.Length <= 10)
                {
                    return true;
                }

                else
                {
                    error = "[R1] Violated: Message Length is greater than 10!";

                    return false;
                }
            }

            // ---------------------------------------------------------------------------------------------------------

            // check wheather R2: messages[i] != current_message, is true or false
            public bool R2CheckNotExisting(string author, string message, AbstractUserMessageList m)
            {
                foreach(AbstractUserMessage msg in m)
                {
                    if((msg.Message == message) && (msg.Author == author))
                    {
                        error = "[R2] Violated: Message already exists!";

                        return false;
                    }
                }
                
                return true;
            }

            // ---------------------------------------------------------------------------------------------------------

            // check wheather R6: if OK => OperationAck, is true or false
            public bool R6CheckOpAck(Type message_type, bool ack)
            {
                if((message_type != typeof(OperationAck)) && (ack == true))
                {
                    error = "[R6] Violated: Model was successfull but no OperationAck was received!";

                    return false;
                }

                else
                {
                    return true;
                }
        
            }

            // ---------------------------------------------------------------------------------------------------------

            // check wheather R7: !OK => OperationFailed, is true or false
            public bool R7CheckOpFail(Type message_type, bool ack)
            {
                if((message_type != typeof(OperationFailed)) && (ack == false))
                {
                    error = "[R7] Violated: Model was unsuccessfull but no OperationFailed was received!";

                    return false;
                }

                else
                {
                    return true;
                }

            }

            // ---------------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------------

        }
    }
}