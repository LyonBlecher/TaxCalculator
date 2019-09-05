using System;
using TaxCalculator.Core.Entities;

namespace TaxCalculator.Core.Exceptions
{
    public class CalculatorNotFoundException : Exception
    {
        public CalculatorNotFoundException(TaxCalculationType taxCalculationType) :base($"Calculator not found for type {taxCalculationType.ToString()}")
        {
        }
    }
}