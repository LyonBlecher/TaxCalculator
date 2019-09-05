namespace TaxCalculator.Core
{
    public interface ITaxCalculator
    {
       decimal CalculateTax(decimal salary, string postalCode);
    }
}