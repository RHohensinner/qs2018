using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsCheckExample
{
    /// <summary>
    /// System under test (simple counter)
    /// </summary>
    public class Counter
    {
        public int Value { get; private set; }
        public void Inc() { Value++; }
        public void Dec() { Value--; }
    }
}
