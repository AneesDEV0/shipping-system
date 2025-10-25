using AutoMapper;
using BusinessLayer.Contracts;
using BusinessLayer.Services;
using DataAccess.Contracts;
using DataAccess.Repositories;
using Domains;
using Microsoft.EntityFrameworkCore;
using Shared.Dtos;

namespace BusinessLayer.Services
{
    public class BaseService<T, DTO> : IBaseService<T,DTO> where T : BaseTable where DTO : BaseDto
    {
        private readonly ITableRepository<T> _repository;
        readonly IMapper _mapper;
        readonly IUserService _userService;
        readonly IUnitOfWork _UnitOfWork;


        public BaseService(ITableRepository<T> repository , IMapper mapper , IUserService userService)
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;

        }
        public BaseService(IUnitOfWork unitofwork, IMapper mapper,
         IUserService userService)
        {
            _UnitOfWork = unitofwork;
            _repository = unitofwork.Repository<T>();
            _mapper = mapper;
            _userService = userService;
        }
        public List<DTO> GetAll()
        {
            var data = _repository.GetAll();    
            return _mapper.Map< IEnumerable<T>,List<DTO>>(data);
        }

        public DTO? GetById(Guid id)
        {
            var obj = _repository.GetById(id);
            return _mapper.Map<T, DTO>(obj);
        }

        public bool Add(DTO entity)
        {
            var dbObject = _mapper.Map<DTO, T>(entity);

            dbObject.CreatedBy =  _userService.GetLoggedInUser();
            dbObject.CurrentState = 1;
      
            return _repository.Add(dbObject);
        }
        public bool Add(DTO entity, out Guid id)
        {
            var dbObject = _mapper.Map<DTO, T>(entity);

            dbObject.CreatedBy = _userService.GetLoggedInUser();
            dbObject.CurrentState = 1;
            return _repository.Add(dbObject , out id);
        }

        public bool Update(DTO entity)
        {
            var dbObject = _mapper.Map<DTO, T>(entity);
            dbObject.CurrentState = 1;
            dbObject.UpdatedBy = _userService.GetLoggedInUser();
            dbObject.UpdatedDate = DateTime.UtcNow;
            return  _repository.Update(dbObject);
        }

       public bool ChangeStatus(Guid id, int stutus = 1)
        {
            return _repository.ChangeStatus(id, _userService.GetLoggedInUser(), stutus);
             
        }

     
    }

}
