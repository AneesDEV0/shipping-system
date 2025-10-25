using AutoMapper;
using BusinessLayer.Contracts;
using Shared.Dtos;
using DataAccess.Contracts;
using DataAccess.Repositories;
using Domains;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class CountryService : BaseService<TbCountry, CountryDto>, ISenderService
    {
        public CountryService(ITableRepository<TbCountry> repo, IMapper mapper,
            IUserService userService) : base(repo, mapper, userService)
        {

        }
    }
}
    