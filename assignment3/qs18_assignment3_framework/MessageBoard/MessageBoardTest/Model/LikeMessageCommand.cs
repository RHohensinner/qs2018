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

        public class LikeMessageCommand : Command<MessageBoard, AbstractUserMessageList>
        {
            string author;
            string message;
            string like_name;
            // string to contain potential specification errors ; will be empty if nothing is found
            string error;

            public LikeMessageCommand(string a, string m, string likeName)
            {
                author = a;
                message = m;
                like_name = likeName;
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

                // init worker and like message
                InitAck initAck = (InitAck)test_client.ReceivedMessages.Dequeue();
                SimulatedActor worker = initAck.Worker;

                // retrieve all messages from author
                RetrieveMessages retrieve_messages = new RetrieveMessages(author, 1);
                worker.Tell(retrieve_messages);
                while(test_client.ReceivedMessages.Count == 0)
                {
                    sa_system.RunFor(1);
                }

                // search for fitting message id
                long message_id = -1;
                FoundMessages found_messages = (FoundMessages)test_client.ReceivedMessages.Dequeue();

                foreach(UserMessage user_message in found_messages.Messages)
                {
                    if(user_message.Message.Equals(message))
                    {
                        message_id = user_message.MessageId;
                    }
                }

                // send Like message
                Like like = new Like(like_name, 1, message_id);
                worker.Tell(like);

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
                AbstractUserMessage au_msg = findMessage(author, message, m);

                // check if message is not found or like does already exist 
                if((au_msg == null) || (au_msg.Likes.Contains(like_name)))
                {
                    // atleast one requirement is hurt set Ack to false and return model
                    m.HasReceivedAck = false;

                    return m;
                }

                // add like to message, set Ack true and return
                au_msg.Likes.Add(like_name);
                m.HasReceivedAck = true;

                return m;
            }

            public override Property Post(MessageBoard a, AbstractUserMessageList m)
            {
                bool valid = true;

                // setting up actual messages
                Message msg = a.Client.ReceivedMessages.Dequeue();

                bool R3 = R3CheckLikeNonExisting(msg.GetType(), m.HasReceivedAck, m);
                bool R6 = R6CheckOpAck(msg.GetType(), m.HasReceivedAck);
                bool R7 = R7CheckOpFail(msg.GetType(), m.HasReceivedAck);

                bool RX = R3 && R6 && R7;

                if(RX == false)
                {
                    valid = false;
                }

                m.HasReceivedAck = false;
                Property valid_prop = valid.ToProperty();
                return valid_prop;
            }

            public override string ToString()
            {
                return "Like(" + author + "," + message + ", liked by:" + like_name +  ")" + " " + error;
            }

            // ---------------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------------
            // HELPER FUNCTIONS:

            // ---------------------------------------------------------------------------------------------------------
            // check wheather R3: non-existing message was liked, is true or false
            public bool R3CheckLikeNonExisting(Type message_type, bool HasReceivedAck, AbstractUserMessageList m)
            {
                bool was_message_found = true;

                AbstractUserMessage au_msg = findMessage(author, message, m);
                if(au_msg == null)
                {
                    was_message_found = false;
                }

                if((HasReceivedAck == false) && (message_type != typeof(OperationFailed) && (was_message_found == false)))
                {
                    error = "[R3] Violated: Non existing message was liked!";

                    return false;
                }

                else
                {
                    return true;
                }
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

            // return the message given by author and message
            public AbstractUserMessage findMessage(string author, string message, AbstractUserMessageList msgs)
            {
                foreach (AbstractUserMessage msg in msgs)
                {
                    if((msg.Message == message) && (msg.Author == author))
                    {
                        return msg;
                    }
                }

                return null;
            }

            // ---------------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------------

        }
    }
}