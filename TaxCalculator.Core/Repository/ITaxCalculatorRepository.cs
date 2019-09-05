using System.Collections.Generic;
using TaxCalculator.Core.Entities;

namespace TaxCalculator.Core.Repository
{
    public interface ITaxCalculatorRepository
    {
        List<TaxTableItem> GetProgressiveTaxTable();
        Dictionary<string, TaxCalculationType> GetTaxCalculationTypes();
        decimal GetFlatRate();
        FlatValue GetFlatValue();
        TaxCalculationType GetTaxCalculationTypeByPostalCode(string postalCode);
        void SaveTaxResult(TaxCalculationType taxCalculationType, decimal salary, string postalCode, decimal taxAmount);
    }
}