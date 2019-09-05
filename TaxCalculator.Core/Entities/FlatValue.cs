using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Core.Entities
{
    public class FlatValue
    {
        public decimal BaseAmount { get; set; }

        public decimal Rate { get; set; }

        public decimal LessThanAmount { get; set; }
    }
}
