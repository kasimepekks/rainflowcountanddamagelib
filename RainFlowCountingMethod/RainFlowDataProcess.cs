using System;
using System.Collections.Generic;
using System.Text;
using RainFlowandDamageTool.Models;

namespace RainFlowandDamageTool.RainFlowCountingMethod
{
    public static class RainFlowDataProcess
    {
        /// <summary>
        /// 雨流计数的结果进行Binning
        /// </summary>
        /// <param name="data">雨流数据源</param>
        /// <param name="dataPara">数据源参数</param>
        /// <returns></returns>
        public static List<RainFlowResult> RangeBining(this List<RainFlowResult> data, DataPara dataPara)
        {
            if (data.Count > 0 && dataPara.binwidth != 0)
            {
                foreach (var item in data)
                {
                    if (item.Range <= dataPara.binwidth)
                    {
                        item.Range = dataPara.binwidth;
                    }
                    else
                    {
                        item.Range = (Math.Floor(item.Range / dataPara.binwidth) + 1) * dataPara.binwidth;
                    }
                }
            }
            return data;
        }
        /// <summary>
        /// 合并雨流数据
        /// </summary>
        /// <param name="data">雨流数据源</param>
        /// <returns></returns>
        public static LinkedList<RainFlowResult> CombineCycle(this LinkedList<RainFlowResult> data)
        {
            //第二层循环解决第一个数据和其他所有的数据的比较，第一层循环就是等第一个数据比较完了再把第二个数据当成第一个再循环比较
            if (data.First != null)
            {
                LinkedListNode<RainFlowResult> cur = data.First;
                LinkedListNode<RainFlowResult> cur2 = data.First;
                while (cur2.Next != null)
                {
                    while (cur.Next != null)
                    {
                        if (Math.Abs(cur2.Value.Range - cur.Next.Value.Range) == 0)
                        {
                            cur2.Value.Count = cur2.Value.Count + cur.Next.Value.Count;
                            data.Remove(cur.Next);
                        }
                        else
                        {
                            cur = cur.Next;
                        }
                    }
                    if (cur2.Next != null)
                    {
                        cur2 = cur2.Next;
                        cur = cur2;
                    }
                }
            }
            return data;
        }
    }
}
