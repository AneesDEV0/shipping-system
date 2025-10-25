using AutoMapper;
using BusinessLayer.Contracts;
using Shared.Dtos;
using DataAccess.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class ShipingPackgingService : BaseService<TbShipingPackging, ShipingPackgingDto>, IShipingPackgingService
    {
        public ShipingPackgingService(ITableRepository<TbShipingPackging> repo, IMapper mapper,
             IUserService userService) : base(repo, mapper, userService)
        {

        }
    }
}
