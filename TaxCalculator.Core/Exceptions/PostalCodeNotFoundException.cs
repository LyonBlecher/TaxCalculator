using System;

namespace TaxCalculator.Core.Exceptions
{
    public class PostalCodeNotFoundException : Exception
    {
        public PostalCodeNotFoundException(string postalCode) : base($"Postal Code {postalCode} not found")
        {
            
        }
    }
}