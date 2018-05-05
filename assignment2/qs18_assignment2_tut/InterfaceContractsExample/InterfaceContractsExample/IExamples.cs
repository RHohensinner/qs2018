using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics.Contracts;

namespace InterfaceContractsExample
{
    [ContractClass(typeof(IExamplesContracts))]
    public interface IExamples
    {
        double sqrt(double x);

        int search(int[] array, int x);
    }

    [ContractClassFor(typeof(IExamples))]
    public abstract class IExamplesContracts : IExamples
    {
        public double sqrt(double x)
        {
            Contract.Requires(x >= 0);

            Contract.Ensures(Math.Abs(x - Contract.Result<double>() * Contract.Result<double>()) < 0.0001);

            return default(double);
        }

        public int search(int[] array, int x)
        {
            Contract.Requires(array != null);

            Contract.Requires(array.Rank == 1);

            Contract.Requires(Contract.ForAll(1, array.Length, i => array[i - 1] < array[i]));

            Contract.Ensures(Contract.Result<int>() < 0 || array[Contract.Result<int>()] == x);

            return default(int);
        }
    }
}
