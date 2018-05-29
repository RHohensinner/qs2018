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

            public PublishCommand(string a, string m)
            {
                author = a;
                message = m;
            }

            public override MessageBoard RunActual(MessageBoard a)
            {
                throw new NotImplementedException();
            }


            public override AbstractUserMessageList RunModel(AbstractUserMessageList m)
            {
                throw new NotImplementedException();
            }

            public override Property Post(MessageBoard a, AbstractUserMessageList m)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return "Publish(" + author + "," + message + ")";
            }
        }

    }
}