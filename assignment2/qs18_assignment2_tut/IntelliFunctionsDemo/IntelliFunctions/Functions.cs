using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliFunctions
{
    public static class Functions
    {

        public static int[] Union(int[] a1, int[] a2)
        {
            List<int> result = new List<int>(a1);
            foreach (var b in a2)
            {
                bool found = false;
                foreach(var a in a1)
                {
                    if (a == b)
                        found = true;
                }

                if (!found)
                    result.Add(b);
            }

            return result.ToArray();
        }

    }
}
