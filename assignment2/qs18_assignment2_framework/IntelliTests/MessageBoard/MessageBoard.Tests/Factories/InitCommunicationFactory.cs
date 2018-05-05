using System;
using Microsoft.Pex.Framework;
using MessageBoard;
using ActorSystem;

namespace MessageBoard
{
    /// <summary>A factory for MessageBoard.InitCommunication instances</summary>
    public static partial class InitCommunicationFactory
    {
        /// <summary>A factory for MessageBoard.InitCommunication instances</summary>
        [PexFactoryMethod(typeof(InitCommunication))]
        public static InitCommunication Create(long communicationId_l)
        {
            InitCommunication initCommunication
               = new InitCommunication(new TestClient(), communicationId_l);
            return initCommunication;
        }
    }
}
