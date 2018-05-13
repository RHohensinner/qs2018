#define CONTRACTS_FULL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace ActorSystem
{
    [ContractClass(typeof(ISimulatedActorSystemContracts))]
    public abstract class ISimulatedActorSystem
    {
        
        /// <summary>
        /// This list contains all actors, which have been started but not stopped.
        /// </summary>
        protected List<SimulatedActor> actors { get; set; }
        /// <summary>
        /// The number of ticks passed since this object was created.
        /// </summary>
        public int currentTime { get; set; }
        /// <summary>
        /// integral number used for creating actor IDs,
        /// which is incremented everytime an actor is started
        /// </summary>
        protected long currentActorId { get; set; }
        /// <summary>
        /// actor ID assigned to new (not yet started) actors
        /// </summary>
        public const long NEW_ACTOR = -1;
        
        public ISimulatedActorSystem()
        {
            Contract.Ensures(this.actors.Count == 0);
            Contract.Ensures(this.currentTime == 0);
            Contract.Ensures(this.currentActorId == 0);

            actors = new List<SimulatedActor>();
            currentTime = 0;
            currentActorId = 0;
        }
        
        public abstract SimulatedActor Spawn(SimulatedActor actor);

        public abstract void RunFor(int numberOfTicks);

        public abstract void RunUntil(int endTime);

        public abstract void Stop(SimulatedActor actor);

        public abstract void Tick();
    }

    /// <summary>
    /// Abstract base class for SimulatedActorSystem class. It is abstract rather than 
    /// an interface to alleviate writing CodeContracts.
    /// 
    /// It is purpose is to manage a list of simulated actors. It is responsible for 
    /// starting and stopping of actors. All active actors in the system
    /// are notified by this class when time units have passed, i.e. 
    /// the <c>ISimulatedActor.Tick()</c> method should only be called
    /// by this class.
    /// </summary>

    [ContractClassFor(typeof(ISimulatedActorSystem))]
    public abstract class ISimulatedActorSystemContracts : ISimulatedActorSystem
    {
        public ISimulatedActorSystemContracts()
        {
            Contract.Ensures(this.actors.Count == 0);
            Contract.Ensures(this.currentTime == 0);
            Contract.Ensures(this.currentActorId == 0);

            actors = new List<SimulatedActor>();
            currentTime = 0;
            currentActorId = 0;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(currentTime >= 0);
            Contract.Invariant(currentActorId == actors.Count);
            Contract.Invariant(Contract.ForAll(0, actors.Count, i => actors[i].TimeSinceSystemStart == currentTime));
        }
        /// <summary>
        /// Starts a new actor, i.e. assigns it an ID, registers
        /// it in the list of active actors and shall call
        /// <c>ISimulatedActor.atStartUp()</c>
        /// </summary>
        /// <param name="actor">actor to be started</param>
        /// <returns>actor which was started</returns>
        public override SimulatedActor Spawn(SimulatedActor actor)
        {
            Contract.Requires(currentTime >= 0);
            Contract.Requires(currentActorId >= 0);
            Contract.Requires(currentActorId >= actors.Count);
            Contract.Requires(!actors.Contains(actor));

            Contract.Ensures(currentActorId >= 0);
            Contract.Ensures(actor.Id >= 0);
            Contract.Ensures(currentActorId >= actors.Count);
            Contract.Ensures(Contract.ForAll(1, actors.Count, i => actors[i].Id == i));
            Contract.Ensures(Contract.ForAll(1, actors.Count, i => actors[i].Id > actors[i - 1].Id));
            Contract.Ensures(actor.TimeSinceSystemStart == currentTime);

            return default(SimulatedActor);
        }

        /// <summary>
        /// Runs the system for the number of ticks (time units)
        /// passed as parameters. The system is run by calling 
        /// <c>ISimulatedActor.Tick()</c> on all active actors.
        /// </summary>
        /// <param name="numberOfTicks">defines how long the system 
        /// should be run</param>
        public override void RunFor(int numberOfTicks)
        {
            int pre_time = currentTime;

            Contract.Requires(numberOfTicks > 0);
            Contract.Requires(currentTime >= 0);

            Contract.Ensures(numberOfTicks > 0);
            Contract.Ensures(currentTime >= 0);

            Contract.Ensures(currentTime == (pre_time + numberOfTicks));

            Contract.Ensures(Contract.ForAll(0, actors.Count, i => actors[i].TimeSinceSystemStart == currentTime));
        }
        // including tick at endTime

        /// <summary>
        /// Runs the system until the current time (time units passed since
        /// object creation) equals the number passed as parameter. 
        /// The last Tick shall be executed when the current time is equal 
        /// to the time passed as parameter. So the current time will actually
        /// be equal to endTime + 1 after the call to this method.
        /// </summary>
        /// <param name="endTime">the target time until which the system should 
        /// be run</param>
        public override void RunUntil(int endTime)
        {
            Contract.Requires(endTime >= 0);
            Contract.Requires(currentTime >= 0);
            Contract.Requires(endTime >= currentTime);

            Contract.Ensures(endTime > 0);
            Contract.Ensures(currentTime >= 0);
            Contract.Ensures(currentTime > endTime);

            Contract.Ensures(currentTime == (endTime + 1));

            Contract.Ensures(Contract.ForAll(0, actors.Count, i => actors[i].TimeSinceSystemStart == currentTime));
        }

        /// <summary>
        /// Stops the actor passed as parameter, by removing it 
        /// from the list of active actors.
        /// </summary>
        /// <param name="actor">the actor to be stopped</param>
        public override void Stop(SimulatedActor actor)
        {
            Contract.Requires(currentTime >= 0);

            Contract.Ensures(!actors.Contains(actor));
        }

        /// <summary>
        /// Helper method used to iterate all actors and 
        /// calling <c>ISimulatedActor.Tick()</c> on it.
        /// </summary>
        public override void Tick()
        {
            Contract.Requires(currentTime >= 0);

            Contract.Ensures(Contract.ForAll(0, actors.Count, i => actors[i].TimeSinceSystemStart == currentTime));
        }
    }

}
