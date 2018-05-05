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
        public int[] SymmetricDifference(int[] array1, int[] array2)
        {
            int[] buf = new int[array1.Length + array2.Length];
            int count = 0;
            bool foundEqualElement = false;

            for (int i = 0; i < array1.Length; i++)
            {
                foundEqualElement = false;
                for (int j = 0; j < array2.Length; j++)
                {
                    if (array1[i] == array2[j])
                    {
                        foundEqualElement = true;
                        break;
                    }
                }
                if (!foundEqualElement)
                {
                    buf[count] = array1[i];
                    count++;
                }
            }

            for (int i = 0; i < array2.Length; i++)
            {
                foundEqualElement = false;
                for (int j = 0; j < array1.Length; j++)
                {
                    if (array2[i] == array1[j])
                    {
                        foundEqualElement = true;
                        break;
                    }
                }
                if (!foundEqualElement)
                {
                    buf[count] = array2[i];
                    count++;
                }
            }

            int[] result = new int[count];
            Array.Copy(buf, result, count);

            return result;
        }


        public Tuple<int, int>[] RelationXIsBiggerThanY(int[] array)
        {
            List<Tuple<int, int>> result = new List<Tuple<int, int>>();

            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    if (array[i] > array[j])
                        result.Add(new Tuple<int, int>(array[i], array[j]));
                }
            }
            return result.ToArray();
        }

        public int[] SortDescending(int[] array)
        {
            int[] sorted = new int[array.Length];
            Array.Copy(array, sorted, array.Length);
            // insertion sort
            for (int i = 0; i < sorted.Length; i++)
            {
                int temp = sorted[i];

                int j = i;
                for (j = i; j > 0 && sorted[j - 1] <= temp; j--)
                {
                    sorted[j] = sorted[j - 1];
                }

                sorted[j] = temp;
            }

            return sorted;
        }
        public int Min(int[] array)
        {
            int min = int.MaxValue;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] < min)
                {
                    min = array[i];
                }
            }
            return min;
        }
        public int MaxIndex(int[] array)
        {
            int maxIndex = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if (maxIndex == -1 || array[i] > array[maxIndex])
                {
                    maxIndex = i;
                }
            }
            return maxIndex;
        }
    }
}
