using AutoMapper;
using BusinessLayer.Contracts;
using Shared.Dtos;
using BusinessLayer.Services;
using DataAccess.Contracts;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserSenderService : BaseService<TbUserSebder, UserSenderDto>, IUserSender
    {
        public UserSenderService(ITableRepository<TbUserSebder> repository, IMapper mapper, IUserService userService , IUnitOfWork unitOfWork) 
            : base(unitOfWork, mapper, userService )
        {
        }
    }
}
