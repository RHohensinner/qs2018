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

        public class FindMessagesCommand : Command<MessageBoard, AbstractUserMessageList>
        {
            string author;
            // string to contain potential specification errors ; will be empty if nothing is found
            string error;

            public FindMessagesCommand(string messageAuthor)
            {
                author = messageAuthor;
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
                if (test_client.ReceivedMessages.Count != 0)
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

                // create retrieve message
                RetrieveMessages retrieve_messages = new RetrieveMessages(author, 1);

                worker.Tell(retrieve_messages);

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
                // there is nothing to be added to the model
                m.HasReceivedAck = true;
                return m;

                /* since testing this command requires (R5) both the actual and the model - testing is done explicitly within the Post property() */
            }

            public override Property Post(MessageBoard a, AbstractUserMessageList m)
            {
                bool validator = true;

                // setting up actual messages
                Message msg = a.Client.ReceivedMessages.Dequeue();
                FoundMessages found_msgs = (FoundMessages)msg;
                List<UserMessage> msgs = found_msgs.Messages;

                bool R1 = R1CheckLength(msgs);
                bool R2 = R2CheckMessageDuplicate(msgs);
                bool R4 = R4CheckDuplicateLikes(msgs);
                bool R5 = R5CheckToString(msgs, m);

                bool RX = R1 && R2 && R4 && R5;

                // check requirements
                if(RX == false)
                {
                    validator = false;
                }
                
                m.HasReceivedAck = false;
                Property valid_prop = validator.ToProperty();
                return valid_prop;
            }

            public override string ToString()
            {
                return "Author(" + author + ")" + " " + error;
            }

            // ---------------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------------
            // HELPER FUNCTIONS:

            // check wheather R1: message length <= 10, is true or false
            public bool R1CheckLength(List<UserMessage> msgs)
            {
                foreach(UserMessage msg in msgs)
                {
                    if(msg.Message.Length > 10)
                    {
                        error = "[R1] Violated: Existing message length is greater than 10!";

                        return false;
                    }
                }

                return true;
            }

            // ---------------------------------------------------------------------------------------------------------

            // check wheather R2: messages[i] != current_message, is true or false
            public bool R2CheckMessageDuplicate(List<UserMessage> msgs)
            {
                // using a SortedSet to check messages for duplicates (SortedSet does not allow duplicates)
                SortedSet<Tuple<string, string>> auth_msgs = new SortedSet<Tuple<string, string>>();

                // for each message create a tuple pair and add it to the SortedSet
                foreach(UserMessage msg in msgs)
                {
                    Tuple<string, string> current_tuple = new Tuple<string, string>(msg.Author, msg.Message);
                    auth_msgs.Add(current_tuple);
                }

                // if two counts do not align, there was atleast one duplicate
                if(msgs.Count != auth_msgs.Count)
                {
                    error = "[R2] Violated: Existing message has duplicate!";

                    return false;
                }

                else
                {
                    return true;
                }
            }

            // ---------------------------------------------------------------------------------------------------------

            // check wheather R4: likes[i].likeNames != likeName, is true or false
            public bool R4CheckDuplicateLikes(List<UserMessage> msgs)
            {
                // same plan as R2
                SortedSet<string> likes = new SortedSet<string>();

                // for each like add it to the SortedSet
                foreach(UserMessage msg in msgs)
                {
                    foreach(string like_author in msg.Likes)
                    {
                        likes.Add(like_author);
                    }

                    if(msg.Likes.Count != likes.Count)
                    {
                        error = "[R4] Violated: One message was liked by the same author atleast twice!";

                        return false;
                    }
                }

                return true;
            }

            // ---------------------------------------------------------------------------------------------------------

            // check wheather R5: output of actual == output of model, is true or false
            public bool R5CheckToString(List<UserMessage> actual_messages, AbstractUserMessageList m)
            {
                string model_output = "";
                string actual_output = "";

                AbstractUserMessageList model_messages = FindMessages(author, m);

                // check actual messages
                foreach(UserMessage actual_message in actual_messages)
                {
                    actual_output += actual_message.ToString();
                }

                // check model messages
                foreach(AbstractUserMessage model_message in model_messages)
                {
                    model_output += model_message.ToString();
                }

                // if they are not the same requirement R5 was hurt
                if(model_output != actual_output)
                {
                    error = "[R5] Violated: Output of ToString() is not the same!";

                    return false;
                }

                return true;
            }

            // ---------------------------------------------------------------------------------------------------------

            // returns a AbstractUserMessageList containing all messages found by given author
            public AbstractUserMessageList FindMessages(string author, AbstractUserMessageList m)
            {
                AbstractUserMessageList found_messages = new AbstractUserMessageList();
                
                foreach(AbstractUserMessage msg in m)
                {
                    if(msg.Author == author)
                    {
                        found_messages.Add(msg);
                    }
                }

                return found_messages;
            }

            // ---------------------------------------------------------------------------------------------------------
            // ---------------------------------------------------------------------------------------------------------

        }
    }
}