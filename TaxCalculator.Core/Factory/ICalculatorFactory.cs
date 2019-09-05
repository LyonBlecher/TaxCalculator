using TaxCalculator.Core.Calculators;
using TaxCalculator.Core.Entities;

namespace TaxCalculator.Core.Factory
{
    public interface ICalculatorFactory
    {
        ICalculator GetCalculator(TaxCalculationType taxCalculationType);
    }
}