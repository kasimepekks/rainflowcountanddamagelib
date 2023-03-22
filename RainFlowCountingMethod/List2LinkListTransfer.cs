using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace RainFlowandDamageTool.RainFlowCountingMethod
{
    public static class List2LinkListTransfer
    {
        /// <summary>
        /// List<T>转成LinkedList<T>
        /// </summary>
        /// <param name="lst">数据源</param>
        /// <returns>LinkedList<T></returns>
        public static LinkedList<T> List2LinkedList <T>(this List<T> lst)
        {
            LinkedList<T> linklist = new LinkedList<T>();
            if (lst.Count > 0)
            {
                foreach (T j in lst)
                {
                    linklist.AddLast(j);
                }
            }
            return linklist;
        }
    }
}
