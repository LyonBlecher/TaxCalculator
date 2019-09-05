namespace TaxCalculator.Core.Entities
{
    public class TaxTableItem
    {
        public decimal From { get; set; }
        public decimal To { get; set; }
        public decimal Rate { get; set; }
    }
}