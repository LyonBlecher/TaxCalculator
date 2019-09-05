using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Options;
using TaxCalculator.Core.Entities;
using TaxCalculator.Core.Options;

namespace TaxCalculator.Core.Datalayer
{
    public class TaxCalculatorDb : ITaxCalculatorDb
    {
        private readonly string _dbConnection;

        public TaxCalculatorDb(IOptions<Settings> settings)
        {
            _dbConnection = settings.Value.DatabaseConnection;
        }

        public void AddCalculatedResult(string postalCode, decimal salary, TaxCalculationType taxCalculationType, decimal taxAmount)
        {
            using (var connection = new SqlConnection(_dbConnection))
            {
                connection.Execute("insert into dbo.[TaxCalculationResult](PostalCode, Salary, TaxCalculationType, TaxAmount)	values(@postalCode, @salary, @taxCalculationType, @taxAmount)", new {postalCode, salary, taxCalculationType, taxAmount});
            }           
        }
    }
}
