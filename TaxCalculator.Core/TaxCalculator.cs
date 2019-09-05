using TaxCalculator.Core.Factory;
using TaxCalculator.Core.Repository;

namespace TaxCalculator.Core
{
    public class TaxCalculator : ITaxCalculator
    {
        private readonly ITaxCalculatorRepository _taxCalculatorRepository;
        private readonly ICalculatorFactory _calculatorFactory;

        public TaxCalculator(ITaxCalculatorRepository taxCalculatorRepository, ICalculatorFactory calculatorFactory)
        {
            _taxCalculatorRepository = taxCalculatorRepository;
            _calculatorFactory = calculatorFactory;
        }       

        public decimal CalculateTax(decimal salary, string postalCode)
        {
            var taxCalculationType = _taxCalculatorRepository.GetTaxCalculationTypeByPostalCode(postalCode);

            var taxAmount = _calculatorFactory.GetCalculator(taxCalculationType).DoCalculation(salary);

            _taxCalculatorRepository.SaveTaxResult(taxCalculationType, salary, postalCode, taxAmount);

            return taxAmount;

        }
    }
}
