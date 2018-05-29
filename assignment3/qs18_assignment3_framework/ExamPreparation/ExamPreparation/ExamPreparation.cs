using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExamPreparation
{
    /// <summary>
    /// Implementation of the functions defined by IExamPreparation
    /// </summary>
    public class ExamPreparation : IExamPreparation
    {
        public int[] Intersect(int[] array1, int[] array2)
        {
            int[] buf = new int[array1.Length];
            int count = 0;

            for (int i = 0; i < array1.Length; i++)
            {
                for (int j = 0; j < array2.Length; j++)
                {
                    if (array1[i] == array2[j])
                    {
                        buf[count] = array1[i];
                        count++;
                    }
                }
            }
            int[] result = new int[count];
            Array.Copy(buf, result, count);

            return result;
        }

        public bool IsProperSubsetOf(int[] subset, int[] superset)
        {
            bool foundItem;

            for (int i = 0; i < subset.Length; i++)
            {
                foundItem = false;
                for (int j = 0; j < superset.Length; j++)
                    if (subset[i] == superset[j])
                        foundItem = true;
                if (foundItem == false)
                    return false;
            }

            if (subset.Length == superset.Length)
                return false;

            return true;
        }

    }
}
