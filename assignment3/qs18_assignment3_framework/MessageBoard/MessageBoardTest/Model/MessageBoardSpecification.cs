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
        public MessageBoard InitialActual
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public AbstractUserMessageList InitialModel
        {
            get
            {
                throw new NotImplementedException();
            }
        }

      
        public Gen<Command<MessageBoard, AbstractUserMessageList>> Next(AbstractUserMessageList value)
        {
            throw new NotImplementedException();
        }
    }
}