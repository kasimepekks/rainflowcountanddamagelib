using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RainFlowandDamageTool.Models;

namespace RainFlowandDamageTool.RainFlowCountingMethod
{
    public static class DataPreprocess
    {
        /// <summary>
        /// 获取数据的最大值，最小值，range，srange，公差等数据
        /// </summary>
        /// <param name="lst">数据源</param>
        /// <returns>DataPara</returns>
        public static DataPara GetDataPara(this List<double> lst)
        {
            DataPara para = new DataPara();
            if (lst.Count > 0)
            {
                //计算实际数据的bin的宽度
                var max = lst.Max();
                var min = lst.Min();
                var maximum = max + (max - min) / 2 / (SNCurvePara.Bins - 1);
                para.maxvalue = maximum;
                var minimum = min - (max - min) / 2 / (SNCurvePara.Bins - 1);
                para.minvalue = minimum;
                para.rfrange = maximum - minimum;
                para.binwidth = para.rfrange / SNCurvePara.Bins;
                para.binwidth = Math.Round(para.binwidth, 4);
                //计算4点法的公差
                var range = max - min;
                para.srange = range;
                var maxdata = Math.Max(Math.Abs(max), Math.Abs(min));
                if (range < 0.00002 * Math.Max(1, maxdata))
                {
                    range = 0.00002 * Math.Max(1, maxdata);
                }
                max = max + 0.1 * range;
                min = min - 0.1 * range;
                para.tollerance = (max - min) / SNCurvePara.Bins;
            }
            return para;
        }
        /// <summary>
        /// 提取峰谷值
        /// </summary>
        /// <param name="lst">数据源</param>
        /// <returns>List<double></returns>
        public static List<double> Findpeaks(this List<double> lst)
        {
            List<double> repv = new List<double>();
            if (lst.Count > 0)
            {
                List<int> diff = new List<int>();
                List<int> diff2 = new List<int>();
                List<int> zeroindex = new List<int>();
                List<int> pvindex = new List<int>();
                //差分计算后一个点-前一个点
                for (int i = 0; i < lst.Count - 1; i++)
                {
                    if (lst[i + 1] - lst[i] > 0)
                    {
                        diff.Add(1);
                    }
                    else if (lst[i + 1] - lst[i] == 0)
                    {

                        diff.Add(0);
                    }
                    else
                    {
                        diff.Add(-1);
                    }
                }
                //如果是0点再看前一个点是否不为0来判断,用2个list来保存需要改为1和-1的index
                List<int> positivelist = new List<int>();
                List<int> nagitivelist = new List<int>();
                for (int i = 1; i < diff.Count; i++)
                {
                    if (diff[i] == 0 && diff[i - 1] < 0)
                    {
                        nagitivelist.Add(i);
                    }
                    else if (diff[i] == 0 && diff[i - 1] > 0)
                    {
                        positivelist.Add(i);
                    }
                }
                //改1和-1
                for (int i = 0; i < positivelist.Count; i++)
                {
                    diff[positivelist[i]] = 1;
                }
                for (int i = 0; i < nagitivelist.Count; i++)
                {
                    diff[nagitivelist[i]] = -1;
                }
                //再进行差分，最后只有-2,0,2。-2为峰，2为谷
                for (int i = 0; i < diff.Count - 1; i++)
                {
                    diff2.Add(diff[i + 1] - diff[i]);
                }
                for (int i = 0; i < diff2.Count; i++)
                {
                    if (diff2[i] == -2 || diff2[i] == 2)
                    {
                        pvindex.Add(i + 1);
                    }
                }
                
                for (int i = 0; i < pvindex.Count; i++)
                {
                    repv.Add(lst[pvindex[i]]);
                }
            }
            return repv;
        }
        /// <summary>
        /// 去除小载荷数据
        /// </summary>
        /// <param name="lst">数据源</param>
        /// <param name="dataPara">数据源参数</param>
        /// <returns>List<double></returns>
        public static List<double> HysteresisFiltering(this List<double> lst, DataPara dataPara)
        {
            if (lst.Count > 0)
            {
                List<int> numlist = new List<int>();
                var max = lst.Max();
                var min = lst.Min();
                for (int i = 0; i < lst.Count - 1; i++)
                {
                    if (Math.Abs(lst[i] - lst[i + 1]) < dataPara.binwidth * SNCurvePara.Filter)
                    {
                        if (lst[i + 1] != max && lst[i + 1] != min)
                        {
                            numlist.Add(i + 1);
                        }
                        
                    }
                }
                //倒序删除List数据
                for (int i = numlist.Count - 1; i >= 0; i--)
                {
                    lst.RemoveAt(numlist[i]);
                }
            }
            return lst;
        }

        public static List<double> HysteresisFilteringV2(this List<double> lst, DataPara dataPara)
        {
          
            var linklist= lst.List2LinkedList();
            LinkedListNode<double>? cur = linklist.First;//获取第一个节点
            if (cur != null)
            {
                //判断是否还有3个点来计算
                while (cur.Next != null && cur.Next.Next != null)
                {
                    //判断第二个点与第一个点的差值是否大于1个bin宽度并且第二个点和第三个点之间的差值是否大于1个bin宽度
                    if (Math.Abs(cur.Next.Value - cur.Value) >= dataPara.binwidth * SNCurvePara.Filter && Math.Abs(cur.Next.Next.Value - cur.Next.Value) >= dataPara.binwidth * SNCurvePara.Filter)
                    {
                        //判读第二个点是否在中间，如果是，则删除这个点且不移动链表，如果不是，则只移动链表
                        if ((cur.Next.Value - cur.Value) * (cur.Next.Value - cur.Next.Next.Value) < 0)
                        {
                            linklist.Remove(cur.Next);
                        }
                        else
                        {
                            //移动到下个节点
                            cur = cur.Next;
                        }


                    }
                    //只要有一个差值不满足则删除幅值为中间的那个点，这个时候不需要移动下个节点
                    else
                    {
                        //获得中间的数并删除那个数
                        var mid = MidNumber.GetMidNumber(cur.Value, cur.Next.Value, cur.Next.Next.Value);
                        if (mid == 1)
                        {
                            cur = cur.Next;
                            if (cur.Previous != null)
                            {
                                linklist.Remove(cur.Previous);
                            }
                           
                        }
                        else if (mid == 2)
                        {
                            linklist.Remove(cur.Next);
                        }
                        else
                        {
                            linklist.Remove(cur.Next.Next);

                        }

                    }

                }
            }
          
            return linklist.ToList();
        }
        /// <summary>
        /// 对数据进行离散化
        /// </summary>
        /// <param name="lst">数据源</param>
        /// <param name="dataPara">数据源的参数</param>
        /// <returns>List<double></returns>
        public static List<double> GetBinning(this List<double> lst, DataPara dataPara)
        {
            if(lst.Count>0 && dataPara.binwidth != 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    lst[i] = dataPara.maxvalue - (Math.Floor((dataPara.maxvalue - lst[i]) / dataPara.binwidth == 1 ? 0 : (dataPara.maxvalue - lst[i]) / dataPara.binwidth) + 0.5) * dataPara.binwidth;
                    // (Math.Floor((lst[i] - dataPara.minvalue) / dataPara.binwidth) + 0.5) * dataPara.binwidth + dataPara.minvalue;
                    // dataPara.maxvalue - (Math.Floor((dataPara.maxvalue - lst[i]) /  dataPara.binwidth == 1 ? 0: (dataPara.maxvalue - lst[i]) / dataPara.binwidth) + 0.5) * dataPara.binwidth;
                }
            }
            return lst;
        }
    }
}
