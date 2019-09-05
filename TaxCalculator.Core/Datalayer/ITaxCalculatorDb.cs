using TaxCalculator.Core.Entities;

namespace TaxCalculator.Core.Datalayer
{
    public interface ITaxCalculatorDb
    {
        void AddCalculatedResult(string postalCode, decimal salary, TaxCalculationType taxCalculationType, decimal taxAmount);
    }
}