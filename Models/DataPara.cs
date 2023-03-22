using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RainFlowandDamageTool.Models
{
    public class DataPara
    {
        public double maxvalue { get; set; }
        public double minvalue { get; set; }
        public double tollerance { get; set; }
        public double srange { get; set; }//信号range
        public double rfrange { get; set; }//计算损伤时的range，比srange要大
        public double binwidth { get; set; }

    }
}
