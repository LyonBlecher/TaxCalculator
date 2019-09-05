namespace TaxCalculator.Core.Calculators
{
    public class FlatRateCalculator : Calculator<decimal>
    {
        public FlatRateCalculator(decimal rateInput) : base(rateInput)
        {
        }

        protected override decimal Calculate(decimal amount)
        {
            return amount * RateInput;
        }       
    }
}
