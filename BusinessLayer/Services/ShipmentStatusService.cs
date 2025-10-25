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

    public class ShipmentStatusService : BaseService<TbShippmentStatus, ShippmentStatusDto>, IShipmentStatus
    {
        IUnitOfWork _uow;
        IUserService _userService;
        ITableRepository<TbShippment> _repo;
        IMapper _mapper;
        public ShipmentStatusService(ITableRepository<TbShippment> repo, IMapper mapper,
             IUserService userService, IUnitOfWork uow) : base(uow, mapper, userService)
        {
            _uow = uow;
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
        }

    }

}
