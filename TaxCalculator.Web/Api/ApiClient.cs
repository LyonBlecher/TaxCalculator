using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TaxCalculator.Api.Contracts;
using TaxCalculator.Web.Options;

namespace TaxCalculator.Web.Api
{
    public class ApiClient : IApiClient
    {
        public ApiClient(IOptions<Options.Settings> settings)
        {
            HttpClient httpClient = new HttpClient();

            Client = new Client(settings.Value.ApiClient, httpClient);
        }

        public IClient Client { get; }
    }
}
