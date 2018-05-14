using System;
using System.Linq;
using System.Diagnostics.Contracts;
namespace ExamPreparation
{
    /// <summary>
    /// Interface providing some functions operating on integer Arrays.
    /// It should be specified using a ContractClass.
    /// </summary>
    [ContractClass(typeof(IExamPreparationContracts))]
    public interface IExamPreparation
    {
        /// <summary>
        /// This method calculates the symmetricDifference (as defined for sets) of a1 and a2.
        /// </summary>
        /// <param name="array1">non-null array containing no duplicate values</param>
        /// <param name="array2">non-null array containing no duplicate values</param>
        /// <returns>union of the parameters</returns>
        int[] SymmetricDifference(int[] array1, int[] array2);

        /// <summary>
        /// Returns all tuples of two elements which satisy the relation: x is bigger than y.
        /// x is the first number of the tuple and y the second number.
        /// </summary>
        /// <param name="array">non-null array containing no duplicate values</param>
        /// <returns>all tuples where x is bigger than y</returns>
        Tuple<int, int>[] RelationXIsBiggerThanY(int[] array);

        /// <summary>
        /// This method sorts the array passed as parameter in descending 
        /// order. It does not sort in-place, i.e. the input-array will 
        /// still be unsorted after the call.
        /// </summary>
        /// <param name="array">non-null array</param>
        /// <returns>a sorted permutation of the input-array</returns>
        int[] SortDescending(int[] array);

        /// <summary>
        /// This method returns the smallest number of the array passed as parameter.
        /// </summary>
        /// <param name="array">non-null array</param>
        /// <returns>maximum of the input-array</returns>
        int Min(int[] array);

        /// <summary>
        /// This method returns the index of the largest number of the array 
        /// passed as parameter. If there are several equally large maxima, 
        /// the index of one of them can be returned.
        /// </summary>
        /// <param name="array">non-null array</param>
        /// <returns>index of a maximum of the input array</returns>
        int MaxIndex(int[] array);
    }


    [ContractClassFor(typeof(IExamPreparation))]
    public abstract class IExamPreparationContracts : IExamPreparation
    {
        public int[] SymmetricDifference(int[] array1, int[] array2)
        {
            Contract.Requires(array1 != null);
            Contract.Requires(array2 != null);
            Contract.Requires(array1.Rank == 1);
            Contract.Requires(array2.Rank == 1);
            Contract.Requires(array1.Count() == array1.Distinct().Count());
            Contract.Requires(array2.Count() == array2.Distinct().Count());

            Contract.Ensures(array1.Except(array2).Union(array2.Except(array1)).Count() == Contract.Result<int[]>().Count());
            Contract.Ensures(array1.Except(array2).Union(array2.Except(array1)).Distinct().Count() == Contract.Result<int[]>().Distinct().Count());
            Contract.Ensures(Contract.ForAll(1, Contract.Result<int[]>().Length, i => Contract.Result<int[]>().Contains(Contract.Result<int[]>()[i])));

            return default(int[]);
        }

        public Tuple<int, int>[] RelationXIsBiggerThanY(int[] array)
        {
            Contract.Requires(array != null);
            Contract.Requires(array.Rank == 1);
            Contract.Requires(array.Distinct().Count() == array.Count());

            Contract.Ensures(Contract.ForAll(1, Contract.Result<Tuple<int, int>[]>().Length, i => Contract.Result<Tuple<int, int>[]>()[i - 1].Item1 > Contract.Result<Tuple<int, int>[]>()[i - 1].Item2));
            Contract.Ensures(Contract.ForAll(1, Contract.Result<Tuple<int, int>[]>().Length, i => array.Contains(Contract.Result<Tuple<int, int>[]>()[i - 1].Item1)));
            Contract.Ensures(Contract.ForAll(1, Contract.Result<Tuple<int, int>[]>().Length, i => array.Contains(Contract.Result<Tuple<int, int>[]>()[i - 1].Item2)));
            
            return default(Tuple<int, int>[]);
        }

        public int[] SortDescending(int[] array)
        {
            Contract.Requires(array != null);
            Contract.Requires(array.Rank == 1);

            Contract.Ensures(Contract.ForAll(1, Contract.Result<int[]>().Length, i => Contract.Result<int[]>()[i - 1] >= Contract.Result<int[]>()[i]));
            Contract.Ensures(Contract.Result<int[]>().Length == array.Length);

            return default(int[]);
        }

        public int Min(int[] array)
        {
            Contract.Requires(array != null);
            Contract.Requires(array.Rank == 1);
            Contract.Requires(array.Length >= 1);
            Contract.Requires((int.MinValue <= array[0]) && (array[0] <= int.MaxValue));

            Contract.Ensures(Contract.ForAll(1, array.Length, i => Contract.Result<int>() <= array[i - 1]));
            Contract.Ensures(array.Contains(Contract.Result<int>()));
            return default(int);
        }

        public int MaxIndex(int[] array)
        {
            Contract.Requires(array != null);
            Contract.Requires(array.Rank == 1);
            Contract.Requires(array.Length >= 1);
            Contract.Requires((int.MinValue <= array[0]) && (array[0] <= int.MaxValue));

            Contract.Ensures(Contract.ForAll(1, array.Length, i => array[Contract.Result<int>()] >= array[i - 1]));
            Contract.Ensures(array.Length >= Contract.Result<int>());
            return default(int);
        }
    }
}

