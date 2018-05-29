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
        /// This method calculates the intersection (as defined for sets) of two arrays.
        /// </summary>
        /// <param name="array1">non-null array containing no duplicate values</param>
        /// <param name="array2">non-null array containing no duplicate values</param>
        /// <returns>intersection of the parameters</returns>
        int[] Intersect(int[] array1, int[] array2);

        /// <summary>
        /// Returns true if the first array passed as a parameter is a proper subset
        /// of the second array passed as a parameter.
        /// </summary>
        /// <param name="subset">non-null array</param>
        /// <param name="superset">non-null array</param>
        /// <returns>boolean indicating if the proper subset relation is satisfied</returns>
        bool IsProperSubsetOf(int[] subset, int[] superset);
    }


    [ContractClassFor(typeof(IExamPreparation))]
    public abstract class IExamPreparationContracts : IExamPreparation
    {
        public int[] Intersect(int[] array1, int[] array2)
        {
            return default(int[]);
        }

        public bool IsProperSubsetOf(int[] subset, int[] superset)
        {
            return default(bool);
        }

    }

}

