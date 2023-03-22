namespace RainFlowandDamageTool.Models
{
    public class RainFlowResult
    {
        public double Fromvalue { get; set; }
        public double Tovalue { get; set; }
        public double Mean { get; set; }
        public double Range { get; set; }
        public double Count { get; set; }
        public double Damage { get; set; }
        public bool Upordown { get; set; }//判断是否上升还是下降，上升为true
        public bool Isresidue { get; set; }//判断是否是residue
        public RainFlowResult(double fromvalue, double tovalue, double mean, double range, double count, bool upordown, bool isresidue, double damage)
        {
            Fromvalue = fromvalue;
            Tovalue = tovalue;
            Mean = mean;
            Range = range;
            Count = count;
            Upordown = upordown;
            Isresidue = isresidue;
            Damage = damage;
        }
    }
}