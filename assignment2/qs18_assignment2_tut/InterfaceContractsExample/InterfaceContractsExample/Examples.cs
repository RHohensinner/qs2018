using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceContractsExample
{
    public class Examples : IExamples
    {
        public double sqrt(double x)
        {
            return Math.Sqrt(x);
        }

        public int search(int[] array, int x)
        {
            return Array.BinarySearch(array, x);
        }
    }


    public class Mutant : IExamples
    {
        public double sqrt(double x)
        {
            return Math.Pow(x, 2);
        }

        public int search(int[] array, int x)
        {
            // TODO: implement mutant... ;-)
            throw new NotImplementedException();
        }
    }

}
