using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QS18
{
    public class Shape
    {
        public static string Triangle(int a, int b, int c)
        {

            if (a < 0 || b < 0 || b < 0 || a > 200 || b > 200 || c > 200)
            {
                ArgumentLogger.addEntryToList(a, b, c, "Input value out of range!");
                throw new ArgumentException("Invalid arguments supplied!");
            }

            string result = "";
            if (a <= c - b || a <= b - c || b <= a - c) result = "no triangle";
            else
                if (a == b && b == c) result = "equilateral"; // b <= c .... may not be killed even by Pex
            else
                  if (a == b || b == c || a == c) result = "isosceles";
            else
                result = "scalene";
            ArgumentLogger.addEntryToList(a, b, c, result);
            return result;
        }
    }
}
