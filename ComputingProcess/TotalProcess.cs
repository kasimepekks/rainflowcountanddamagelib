using RainFlowandDamageTool.RainFlowCountingMethod;
using System;
using System.Collections.Generic;
using System.Text;

namespace RainFlowandDamageTool.ComputingProcess
{
    public static class TotalProcess
    {
        /// <summary>
        /// 获得累积损伤（repeated block）
        /// </summary>
        /// <param name="lst">数据源</param>
        /// <returns>累计损伤</returns>
        public static double GetAccumDamagefromList(this List<double> lst,double k1,double k2)
        {
            double normaldamage = 0;
            double residuedamage = 0;
            if (lst != null)
            {
                var datapara = lst.GetDataPara();//获得源数据的各种参数
                normaldamage = lst.Findpeaks().HysteresisFilteringV2(datapara).GetBinning(datapara).List2LinkedList()
                    .GetCycle(datapara,false, out List<double>residue).List2LinkedList().CombineCycle()
                    .GetDamage(k1,k2).GetAccumDamage();//获得第一次的雨流计数结果和residue
                residuedamage = residue.DuplicateResidue().Findpeaks().List2LinkedList()
                    .GetCycle(datapara, true, out List<double> useless)
                    .List2LinkedList().CombineCycle().GetDamage(k1,k2).GetAccumDamage();//获得residue的雨流计数结果
            }
            return normaldamage+residuedamage;
        }
    }
}
