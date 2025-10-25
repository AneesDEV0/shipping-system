
using Shared.Dtos;
using BusinessLayer.Services;
using Domains;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Contracts
{
    public interface IShippingTypeService : IBaseService<TbShippingType, ShippingTypeDto>
    {
    }
}
    