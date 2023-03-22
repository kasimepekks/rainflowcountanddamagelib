using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainFlowandDamageTool.Models
{
    public class SNCurvePara
    {
        public static double Smax { get; set; } = 100000000;
        public static double S1 { get; set; } = 1000;
        public static double SE { get; set; } = 1000;
        public static double K1 { get; set; } = 5;
        public static double K2 { get; set; } = 5;
        public static double NE { get; set; } = 10000000;
        public static double N1 { get; set; } = Math.Max(NE * Math.Pow(S1 / SE, -K2), 1);
        public static double Bins { get; set; } = 100;
        public static double Filter { get; set; } = 1;

        /// <summary>
        /// 修改参数方法
        /// </summary>
        /// <param name="smax"></param>
        /// <param name="s1"></param>
        /// <param name="se"></param>
        /// <param name="k1"></param>
        /// <param name="k2"></param>
        /// <param name="ne"></param>
        /// <param name="bins"></param>
        /// <param name="filter"></param>
        public static void EditSNPara(double smax, double s1, double se, double k1, double k2, double ne, double bins, double filter)
        {
            Smax = smax;
            S1 = s1;
            SE = se;
            K1 = k1;
            K2 = k2;
            NE = ne;
            Bins = bins;
            Filter = filter;
            N1 = Math.Max(NE * Math.Pow(S1 / SE, -K2), 1);

        }

    }
}
