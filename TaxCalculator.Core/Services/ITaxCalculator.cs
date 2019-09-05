namespace TaxCalculator.Core.Services
{
    public interface ITaxCalculator
    {
       decimal CalculateTax(decimal salary, string postalCode);
    }
}