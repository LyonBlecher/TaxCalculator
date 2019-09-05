using TaxCalculator.Core.Calculators;

namespace TaxCalculator.Core.Services
{
    public interface ICalculatorFactory
    {
        ICalculator GetCalculator(TaxCalculationType taxCalculationType);
    }
}