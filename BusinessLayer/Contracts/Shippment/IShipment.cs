using BusinessLayer.Services;
using DataAccess.Models;
using Domains;
using Shared.Dtos;


namespace BusinessLayer.Contracts
{
    public interface IShipment : IBaseService<TbShippment, ShipmentDto>
    {
        public Task Create(ShipmentDto dto);
        public Task Edit(ShipmentDto dto);
        public Task<List<ShipmentDto>> GetShipments();

        public Task<PagedResult<ShipmentDto>> GetShipments(int pageNumber, int pageSize);

        public Task<ShipmentDto> GetShipment(Guid id);

    }
}
