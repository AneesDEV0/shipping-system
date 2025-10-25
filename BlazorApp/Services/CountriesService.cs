using BlazorApp.Contracts;
using BlazorApp.Services.@base;
using Shared.Dtos;

namespace BlazorApp.Services
{
    public class CountriesService : BaseService<CountryDto>, ICountriesService
    {
        public CountriesService(HttpClient http) : base(http)
        {
        }
        public override string _endpoint => "api/Countries";
    }
}
