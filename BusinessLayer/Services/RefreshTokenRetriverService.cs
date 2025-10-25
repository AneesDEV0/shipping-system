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
    public class RefreshTokenRetriverService : IRefreshTokenRetriver
    {
        ITableRepository<TbRefreshTokens> _repo;
        IMapper _mapper;
        public RefreshTokenRetriverService(ITableRepository<TbRefreshTokens> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public RefreshTokenDto GetByToken(string token)
        {
            var refreshToken = _repo.GetFirstOrDefault(a => a.Token == token);
            return _mapper.Map<TbRefreshTokens, RefreshTokenDto>(refreshToken);
        }
    }

}
