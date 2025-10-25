using AutoMapper;
using BusinessLayer.Contracts;
using Shared.Dtos;
using BusinessLayer.Services;
using DataAccess.Contracts;
using DataAccess.Repositories;
using Domains;
using Shared.Dtos;

public class UserReceverService : BaseService<TbUserReceiver, UserReceiverDto>, IUserRecever
{
    public UserReceverService(ITableRepository<TbUserReceiver> repository, IMapper mapper, IUserService userService , IUnitOfWork unitOfWork) : base(unitOfWork, mapper, userService)
    {
    }
}