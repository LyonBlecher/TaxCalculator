using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxCalculator.Web.Models
{
    public class CalculateModel
    {
        [Display(Name = "Postal Code")]
        [Required]
        [StringLength(10)]
        public string PostalCode { get; set; }

        [Display(Name = "Salary")]
        [Required]
        public double Salary { get; set; }

        public double TaxAmount { get; set; }

        public double CashLeft => Salary - TaxAmount;
        public bool Calculated { get; set; }

        public string ErrorMessage { get; set; }
    }   
}