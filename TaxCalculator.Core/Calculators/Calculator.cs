using System;

namespace TaxCalculator.Core.Calculators
{
    public abstract class Calculator<TRateInput> : ICalculator
    {
        protected TRateInput RateInput { get; }

        protected Calculator(TRateInput rateInput)
        {
            RateInput = rateInput;
        }

        protected abstract decimal Calculate(decimal salary);

        public decimal DoCalculation(decimal salary)
        {
            if (salary <= 0)
            {
                throw new ArgumentOutOfRangeException( nameof(salary), "salary must be greater than 0");
            }

            return Calculate(salary);
        }
    }
}