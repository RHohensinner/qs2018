using ActorSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard
{
    /// <summary>
    /// System Under Test. Should be used as Actual parameter in the Commands.
    /// </summary>
    public class MessageBoard
    {
        public SimulatedActorSystem System { get; set; }
        public Dispatcher Dispatcher { get; set; }
        public TestClient Client { get; set; }

        public MessageBoard()
        {
            // init system client and dispatcher
            System = new SimulatedActorSystem();
            Client = new TestClient();
            Dispatcher = new Dispatcher(System, 1);

            // spawning client and dispatcher
            System.Spawn(Client);
            System.Spawn(Dispatcher);
        }
    }
}
