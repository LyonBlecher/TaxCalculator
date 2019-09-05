using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TaxCalculator.Api.Contracts;
using TaxCalculator.Web.Api;
using TaxCalculator.Web.Models;

namespace TaxCalculator.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IApiClient _apiClient;

        public HomeController(IApiClient apiClient, IOptions<Options.Settings> settings)
        {
            _apiClient = apiClient;


        }

        public async Task<IActionResult> Index(CalculateModel model)
        {

            //if (!string.IsNullOrEmpty(model.PostalCode) && model.Salary > 0)
            //{
            if (ModelState.IsValid)
            {
                try
                {
                    var taxAmount = await _apiClient.Client.CalculateAsync(Convert.ToDouble(model.Salary), model.PostalCode);

                    model.TaxAmount = taxAmount;
                    model.Calculated = true;
                }
                catch (ApiException e)
                {
                    model.ErrorMessage = JsonConvert.DeserializeObject<ApiErrorModel>(e.Response).Error;
                }

            }
            //}

            return View(model);
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Calculate(CalculateModel model)
        {
            var taxAmount = await _apiClient.Client.CalculateAsync(Convert.ToDouble(model.Salary), model.PostalCode);

            model.TaxAmount = taxAmount;

            return View(model);
        }
    }
}
