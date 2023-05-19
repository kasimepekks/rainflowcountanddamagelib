using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using RainFlowandDamageTool.Models;
using static System.Net.Mime.MediaTypeNames;

namespace RainFlowandDamageTool.RainFlowCountingMethod
{
    public static class DamageCalculation
    {
        /// <summary>
        /// 计算每个雨流数据的损伤
        /// </summary>
        /// <param name="data">雨流数据源</param>
        /// <returns></returns>
        public static LinkedList<RainFlowResult> GetDamage(this LinkedList<RainFlowResult> data,double k1,double k2)
        {
            SNCurvePara.EditSNPara(k1, k2);
            if (data.Count > 0)
            {
                foreach (var i in data)
                {
                    double nrange = i.Range / 2;
                    if (nrange <= SNCurvePara.SE)
                    {
                        i.Damage = i.Count * Math.Pow((nrange / SNCurvePara.SE), SNCurvePara.K2) / SNCurvePara.NE;
                    }
                    else if (nrange > SNCurvePara.SE && nrange <= SNCurvePara.S1)
                    {
                        i.Damage = i.Count * Math.Pow((nrange / SNCurvePara.SE), SNCurvePara.K2) / SNCurvePara.NE;
                    }
                    else if (nrange > SNCurvePara.S1 && nrange <= SNCurvePara.Smax)
                    {
                        i.Damage = i.Count * Math.Pow((nrange / SNCurvePara.S1), SNCurvePara.K1) / SNCurvePara.N1;
                    }
                }
            }
            return data;
        }
        /// <summary>
        /// 获得累计损伤
        /// </summary>
        /// <param name="data">雨流数据源</param>
        /// <returns></returns>
        public static double GetAccumDamage(this LinkedList<RainFlowResult> data)
        {
            if (data.Count>0)
            {
                double t = 0;
                foreach(var i in data)
                {
                    t = t + i.Damage;
                }
                return t;
            }
            else
            {
                return 0;
            }
        }
    }
}
