using TaxCalculator.Api.Contracts;

namespace TaxCalculator.Web.Api
{
    public interface IApiClient
    {
        IClient Client { get; }
    }
}