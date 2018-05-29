using FsCheck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsCheckExample
{
    /// <summary>
    /// specification
    /// </summary>
    public class CounterSpec : ICommandGenerator<Counter, int>
    {
        /// <summary>
        /// should generate the next command in the command sequence
        /// </summary>
        /// <param name="value">current model value</param>
        /// <returns>a generator that selects a command</returns>
        public Gen<Command<Counter, int>> Next(int value)
        {
            return Gen.Elements(new Command<Counter, int>[] { new Inc(), new Dec() });
        }

        /// <summary>
        /// initialization of the SUT
        /// </summary>
        public Counter InitialActual { get { return new Counter(); } }

        /// <summary>
        /// initialization of the model
        /// </summary>
        public int InitialModel { get { return 0; } }

    }
}
