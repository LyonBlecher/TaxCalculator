using System.Collections.Generic;
using TaxCalculator.Core.Entities;
using TaxCalculator.Core.Repository;

namespace TaxCalculator.Core.Calculators
{
    public class ProgressiveCalculator : Calculator<List<TaxTableItem>>
    {
        public ProgressiveCalculator(List<TaxTableItem> rateInput) : base(rateInput)
        {
        }

        protected override decimal Calculate(decimal salary)
        {
            var tax = 0M;

            foreach (var tableItem in RateInput)
            {
                if (salary >= tableItem.From && salary < tableItem.To + 1 || salary >= tableItem.From && tableItem.To == TaxCalculatorRepository.MaxProgressiveAmount)
                {
                    tax += (salary - tableItem.From) * tableItem.Rate;
                    break;
                }

                tax += (tableItem.To - tableItem.From) * tableItem.Rate;
            }

            return tax;
        }


    }
}
