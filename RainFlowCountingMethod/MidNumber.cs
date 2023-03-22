using System;
using System.Collections.Generic;
using System.Text;

namespace RainFlowandDamageTool.RainFlowCountingMethod
{
    public static class MidNumber
    {
        /// <summary>
        /// 给3个值，返回中间的index
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int GetMidNumber(double a, double b, double c)
        {
            if (a == b)
            {
                return 2;
            }
            else if (a == c || b == c)
            {
                return 3;
            }

            else if ((a > b && a < c) || (a < b && a > c))
            {
                return 1;
            }
            else if ((b > a && b < c) || (b < a && b > c))
            {
                return 2;
            }
            else
            {
                return 3;
            }

        }
    }
}
