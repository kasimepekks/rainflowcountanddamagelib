using System;
using System.Collections.Generic;
using System.Text;
using RainFlowandDamageTool.Models;

namespace RainFlowandDamageTool.RainFlowCountingMethod
{
    public static class RainFlowCounting
    {
        /// <summary>
        /// 获取雨流计数和residue
        /// </summary>
        /// <param name="data">数据源</param>
        /// <param name="dataPara">容忍公差</param>
        /// <param name="isresidue">标识是否为residue</param>
        /// <param name="residue">输出residue数据</param>
        /// <returns></returns>
        public static List<RainFlowResult> GetCycle(this LinkedList<double> data, DataPara dataPara, bool isresidue, out List<double> residue)
        {
            int count = data.Count;
            List<RainFlowResult> lst = new List<RainFlowResult>();
            if (data.First!=null && count>3)
            {
                LinkedListNode<double> cur = data.First;//获取第一个节点
               
                //用4点法计算循环数
                while (count > 3 && cur.Next != null && cur.Next.Next!=null && cur.Next.Next.Next != null)//4个点以上才能用4点法
                {
                    //先判断是否是上升4点
                    if (cur.Next.Value > cur.Value)
                    {
                        //再根据上升4点法的规则进行判断
                        if (cur.Next.Next.Next.Value >= cur.Next.Value && cur.Next.Next.Value >= cur.Value - dataPara.tollerance)
                        {
                            double mean = (cur.Next.Next.Value + cur.Next.Value) / 2;
                            double range = Math.Abs(cur.Next.Next.Value - cur.Next.Value);
                            double rfcount = 1;
                            RainFlowResult result = new RainFlowResult(cur.Next.Value, cur.Next.Next.Value, mean, range, rfcount, true, isresidue, 0);
                            lst.Add(result);
                            data.Remove(cur.Next);
                            data.Remove(cur.Next);
                            count = count - 2;
                            cur = data.First;//有符合条件的4点就从头开始进行判断
                        }
                        //如果不满足4点法则判断下一个4点
                        else
                        {
                            cur = cur.Next;
                            //count--;//这里不能减数量，因为会提前结束4点法判断
                        }
                    }
                    //如果是下降4点
                    else
                    {
                        //再根据下降4点法的规则进行判断
                        if (cur.Next.Next.Next.Value <= cur.Next.Value + dataPara.tollerance && cur.Next.Next.Value <= cur.Value)
                        {
                            double mean = (cur.Next.Next.Value + cur.Next.Value) / 2;
                            double range = Math.Abs(cur.Next.Next.Value - cur.Next.Value);
                            double rfcount = 1;
                            RainFlowResult result = new RainFlowResult(cur.Next.Value, cur.Next.Next.Value, mean, range, rfcount, false, isresidue, 0);
                            lst.Add(result);
                            data.Remove(cur.Next);
                            data.Remove(cur.Next);
                            count = count - 2;
                            cur = data.First;
                        }
                        //如果不满足4点法则判断下一个4点
                        else
                        {
                            cur = cur.Next;
                            //count--;
                        }
                    }
                }
            }
            residue = data.ToList();
            return lst;
        }

    }
}
