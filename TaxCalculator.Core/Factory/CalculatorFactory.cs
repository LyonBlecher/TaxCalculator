using TaxCalculator.Core.Calculators;
using TaxCalculator.Core.Entities;
using TaxCalculator.Core.Exceptions;
using TaxCalculator.Core.Repository;

namespace TaxCalculator.Core.Factory
{
    public class CalculatorFactory : ICalculatorFactory
    {
        private readonly ITaxCalculatorRepository _taxCalculatorRepository;

        public CalculatorFactory(ITaxCalculatorRepository taxCalculatorRepository)
        {
            _taxCalculatorRepository = taxCalculatorRepository;
        }

        public ICalculator GetCalculator(TaxCalculationType taxCalculationType)
        {
            switch (taxCalculationType)
            {
                case TaxCalculationType.FlatRate:
                    return new FlatRateCalculator(_taxCalculatorRepository.GetFlatRate());
                case TaxCalculationType.Progressive:
                    return new ProgressiveCalculator(_taxCalculatorRepository.GetProgressiveTaxTable());
                case TaxCalculationType.FlatValue:
                    return new FlatValueCalculator(_taxCalculatorRepository.GetFlatValue());
            }

            throw new CalculatorNotFoundException(taxCalculationType);
        }
    }
}
