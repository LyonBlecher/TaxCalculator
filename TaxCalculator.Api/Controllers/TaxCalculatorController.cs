using System.Net;
using Microsoft.AspNetCore.Mvc;
using TaxCalculator.Core;

namespace TaxCalculator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxCalculatorController : ControllerBase
    {
        
        private readonly ITaxCalculator _taxCalculator;

        public TaxCalculatorController(ITaxCalculator taxCalculator)
        {
            _taxCalculator = taxCalculator;
        }

        /// <summary>
        /// Calculate tax based on salary and postal code
        /// </summary>
        /// <param name="salary"></param>
        /// <param name="postalCode"></param>
        [HttpPost("Calculate")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult<decimal> Calculate(decimal salary, string postalCode)
        {
            return _taxCalculator.CalculateTax(salary, postalCode);
        }        
    }
}
