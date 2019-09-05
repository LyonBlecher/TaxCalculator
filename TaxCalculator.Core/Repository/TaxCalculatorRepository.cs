using System;
using System.Collections.Generic;
using TaxCalculator.Core.Datalayer;
using TaxCalculator.Core.Entities;
using TaxCalculator.Core.Exceptions;

namespace TaxCalculator.Core.Repository
{
    public class TaxCalculatorRepository : ITaxCalculatorRepository
    {
        private readonly ITaxCalculatorDb _taxCalculatorDb;
        public const decimal MaxProgressiveAmount = -1;

        public TaxCalculatorRepository(ITaxCalculatorDb taxCalculatorDb)
        {
            _taxCalculatorDb = taxCalculatorDb;
        }

        public List<TaxTableItem> GetProgressiveTaxTable()
        {
            return new List<TaxTableItem>
            {
                new TaxTableItem{ From = 0, To = 8350, Rate = 0.10M },
                new TaxTableItem{ From = 8351, To = 33950, Rate = 0.15M },
                new TaxTableItem{ From = 33951, To = 88250, Rate = 0.25M },
                new TaxTableItem{ From = 88251, To = 171550, Rate = 0.28M },
                new TaxTableItem{ From = 171551, To = 372950, Rate = 0.33M },
                new TaxTableItem{ From = 372951, To = MaxProgressiveAmount, Rate = 0.35M }
            };
        }

        public Dictionary<string, TaxCalculationType> GetTaxCalculationTypes()
        {
            return new Dictionary<string, TaxCalculationType>
            {
                { "7441", TaxCalculationType.Progressive },
                { "A100", TaxCalculationType.FlatValue },
                { "7000", TaxCalculationType.FlatRate },
                { "1000", TaxCalculationType.Progressive }

            };
        }

        public decimal GetFlatRate()
        {
            return 0.175M;
        }

        public FlatValue GetFlatValue()
        {
            return new FlatValue
            {
                BaseAmount = 10000,
                Rate = 0.05M,
                LessThanAmount = 200000
            };
        }

        public TaxCalculationType GetTaxCalculationTypeByPostalCode(string postalCode)
        {
            var exists = GetTaxCalculationTypes().TryGetValue(postalCode, out var taxCalculationType);

            if (!exists)
            {
                throw new PostalCodeNotFoundException(postalCode);
            }

            return taxCalculationType;
        }

        public void SaveTaxResult(TaxCalculationType taxCalculationType, decimal salary, string postalCode, decimal taxAmount)
        {          
            _taxCalculatorDb.AddCalculatedResult(postalCode, salary, taxCalculationType, taxAmount);
        }
    }
}
