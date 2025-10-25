using AutoMapper;
using BusinessLayer.Contracts;
using Shared.Dtos;
using BusinessLayer.Services;
using DataAccess.Contracts;
using Domains;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CityService : BaseService<TbCity, CityDto>, ICityService
    {
        IViewRepository<VwCities> _ViewRepo;
        IMapper _mapper;
        public CityService(ITableRepository<TbCity> repo, IMapper mapper,
          IUserService userService, IViewRepository<VwCities> viewRepo) : base(repo, mapper, userService)
        {
            _ViewRepo = viewRepo;
            _mapper = mapper;
        }

        public List<CityDto> GetAllCitites()
        {
            var cities = _ViewRepo.GetList(a => a.CurrentState == 1).ToList();
            return _mapper.Map<List<VwCities>, List<CityDto>>(cities);
        }
        // GetByCountry
        public List<CityDto> GetByCountry(Guid countryId)
        {
            var data = _ViewRepo.GetList(x => x.CountryId == countryId);
            return _mapper.Map<List<VwCities>, List<CityDto>>(data);
        }
    }
}
