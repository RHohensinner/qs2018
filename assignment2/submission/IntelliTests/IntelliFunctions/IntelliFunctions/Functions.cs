using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliFunctions
{
    public static class Functions
    {
        public static int[] SortDescending(int[] array)
        {
            int[] result = new int[array.Length];
            Array.Copy(array, result, array.Length);

            for (int i = 0; i < result.Length; i++)
            {
                int min = i;
                for (int j = i + 1; j < result.Length; j++)
                {
                    if (result[j] > result[min])
                        min = j;
                }

                int tmp = result[min];
                result[min] = result[i];
                result[i] = tmp;
            }
            return result;
        }

        public static bool IsSubsetOf(int[] subset, int[] superset)
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

            subset = new int[] { };
            return true;
        }

        public static int[][] CombinationWithoutRepetition(int[] set, int numberOfElements)
        {
            int[][] res = CombinationWithoutRepetitionInner(set, numberOfElements).ToArray();

            return res;
        }

        private static IEnumerable<int[]> CombinationWithoutRepetitionInner(int[] set, int numberOfElements)
        {
            int i = 0;
            foreach (var e in set)
            {
                if (numberOfElements == 1)
                    yield return new int[] { e };
                else
                {
                    foreach (var result in CombinationWithoutRepetitionInner(set.Skip(i + 1).ToArray(), numberOfElements - 1))
                        yield return new int[] { e }.Concat(result).ToArray();
                }
                i++;
            }
        }
    }
}
