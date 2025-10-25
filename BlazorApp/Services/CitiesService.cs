using BlazorApp.Contracts;
using BlazorApp.Services.@base;
using Shared.Dtos;

namespace BlazorApp.Services
{
    public class CitiesService : BaseService<CityDto>, ICitiesService
    {
        public CitiesService(HttpClient http) : base(http)
        {
        }
        public override string _endpoint => "api/Cities";
       
    }
}
