using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RainFlowandDamageTool.RainFlowCountingMethod
{
    public static class ResidueProcess
    {
        /// <summary>
        /// 复制residue数据到residue数据
        /// </summary>
        /// <param name="data">数据源</param>
        /// <returns></returns>
        public static List<double> DuplicateResidue(this List<double> data)
        {
            var ct = data.Count();
            if(ct > 0)
            {
                for (int i = 0; i < ct; i++)
                {
                    data.Add(data[i]);
                }
            }
            return data;
        }
    }
}
