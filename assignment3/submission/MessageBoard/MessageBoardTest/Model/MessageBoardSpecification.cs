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
        // initializing a random system to create test data
        public System.Random rand_system = new System.Random();
        // assuming author, messages and likenames consist of only upper and lower case letters no numbers etc.
        const string valid_characters = "aAbBcCdDeEfFgGhHiIjJkKlLmMnNoOpPqQrRsStTuUvVwWxXyYzZ";

        string author = "";
        string message = "";
        string likename = "";

        public MessageBoard InitialActual
        {
            get
            {
                // use MessageBoard for SUT
                return new MessageBoard();
            }
        }

        public AbstractUserMessageList InitialModel
        {
            get
            {
                // use AbstractUserMessageList for model
                return new AbstractUserMessageList();
            }
        }


        public Gen<Command<MessageBoard, AbstractUserMessageList>> Next(AbstractUserMessageList value)
        {
            // creating test data
            List<string> test_data = CreateTestData();

            // creating random indices
            int a, b, c = 0;
            a = rand_system.Next(0, 199);
            b = rand_system.Next(0, 199);
            c = rand_system.Next(0, 199);

            // setting author, message and likename strings
            author = test_data.ElementAt(a);
            message = test_data.ElementAt(b);
            likename = test_data.ElementAt(c);

            return Gen.Elements(new Command<MessageBoard, AbstractUserMessageList>[] 
            {
                // 1 random publish command and 1 hardcoded publish command to ensure duplicates are tested
                new PublishCommand("Richard", "Hello"),
                new PublishCommand(author, message),

                // 1 random find message command
                new FindMessagesCommand(author),
                
                // 1 random like message command and 1 hardcoded like to ensure duplicate likes are tested
                new LikeMessageCommand("Richard", "Hello", "Maria"),
                new LikeMessageCommand(author, message, likename)

            });
        }

        // creates test data 
        public List<string> CreateTestData()
        {
            List<string> test_data = new List<string>();
            int counter;

            // each iteration creates 200 teststrings
            for(counter = 0; counter < 200; counter++)
            {
                // message lengths is between 1 and 20, so messages could be longer than 10 => fail
                int data_length = rand_system.Next(1, 20);

                // select chars from valid_chars until desired length is reached
                string test_string = new string (Enumerable.Repeat(valid_characters, data_length)
                                              .Select(i => i[rand_system.Next(i.Length)]).ToArray());
                // add test string to data set
                test_data.Add(test_string);
            }

            return test_data;
        }
    }
}