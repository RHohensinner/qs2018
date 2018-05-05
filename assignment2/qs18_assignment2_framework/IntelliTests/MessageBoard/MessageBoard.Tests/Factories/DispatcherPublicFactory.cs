using System;
using Microsoft.Pex.Framework;
using MessageBoard;
using ActorSystem;
using System.Linq;

namespace MessageBoard
{
    /// <summary>A factory for MessageBoard.DispatcherPublic instances</summary>
    public static partial class DispatcherPublicFactory
    {
        /// <summary>A factory for MessageBoard.DispatcherPublic instances</summary>
        [PexFactoryMethod(typeof(DispatcherPublic))]
        public static DispatcherPublic Create(int numberOfWorkers_i, int mode_selector)
        {
            PexAssume.IsTrue(0 < numberOfWorkers_i && numberOfWorkers_i < 10);

            DispatcherPublic.Mode mode = mode_selector % 2 == 0 ? Dispatcher.Mode.STOPPING : Dispatcher.Mode.NORMAL; 
            SimulatedActorSystemPublic system = new SimulatedActorSystemPublic();
            DispatcherPublic testDispatcher
               = new DispatcherPublic(system, numberOfWorkers_i);
            testDispatcher.ModeProp = mode;
            system.Spawn(testDispatcher);
            if (mode == Dispatcher.Mode.STOPPING)
            {
                testDispatcher.AcksToCollect = testDispatcher.Worker.Select(w => w.Id).ToList();
            }
            testDispatcher.TestSystem = system;
            return testDispatcher;
        }
    }
}
