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
            return default(int[]);
        }

        public Tuple<int, int>[] RelationXIsBiggerThanY(int[] array)
        {
            return default(Tuple<int, int>[]);
        }

        public int[] SortDescending(int[] array)
        {
            return default(int[]);
        }

        public int Min(int[] array)
        {
            return default(int);
        }

        public int MaxIndex(int[] array)
        {
            return default(int);
        }
    }

}

