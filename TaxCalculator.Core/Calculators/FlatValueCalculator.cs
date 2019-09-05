using TaxCalculator.Core.Entities;

namespace TaxCalculator.Core.Calculators
{
    public class FlatValueCalculator : Calculator<FlatValue>
    {
        public FlatValueCalculator(FlatValue rateInput) : base(rateInput)
        {
        }

        protected override decimal Calculate(decimal salary)
        {
            if (salary < RateInput.LessThanAmount)
            {
                return salary * RateInput.Rate;
            }

            return RateInput.BaseAmount;
        }       
    }
}
