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
    public class RefreshTokenService : BaseService<TbRefreshTokens, RefreshTokenDto>, IRefreshTokens
    {
        ITableRepository<TbRefreshTokens> _repo;
        IMapper _mapper;
        public RefreshTokenService(ITableRepository<TbRefreshTokens> repository, IMapper mapper, IUserService userService) : base(repository, mapper, userService)
        {
            _repo = repository;
            _mapper = mapper;
        }
        public RefreshTokenDto GetByToken(string token)
        {
            var refreshToken = _repo.GetFirstOrDefault(a => a.Token == token);
            return _mapper.Map<TbRefreshTokens, RefreshTokenDto>(refreshToken);
        }

        public async Task<bool> Refresh(RefreshTokenDto tokenDto)
        {
            var tokenList = await _repo.GetList(a => a.UserId == tokenDto.UserId && a.CurrentState == 1);
            foreach (var dbToken in tokenList)
            {
                _repo.ChangeStatus(dbToken.Id, Guid.Parse(tokenDto.UserId), 2);
            }

            var dbTokens = _mapper.Map<RefreshTokenDto, TbRefreshTokens>(tokenDto);
            _repo.Add(dbTokens);
            return true;
        }
    }
}
