
    using AutoMapper;
    using BusinessLayer.Contracts;
    using Shared.Dtos;
    using DataAccess.Contracts;
    using Domains;

    namespace BusinessLayer.Services
    {
        public class ShippingTypeService
            : BaseService<TbShippingType,ShippingTypeDto>,
              IShippingTypeService
        {
            public ShippingTypeService(ITableRepository<TbShippingType> shippingtype , IMapper maper, IUserService userService)
                : base(shippingtype , maper , userService)
            {
            }
        }
    }
