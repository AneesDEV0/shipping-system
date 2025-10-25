using BusinessLayer.Contracts;
using Shared.Dtos;
using DataAccess.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingPackgingController : ControllerBase
    {
        IShipingPackgingService _shipingPackging;
        public ShippingPackgingController(IShipingPackgingService shipingPackging)
        {
            _shipingPackging = shipingPackging;
        }
        // GET: api/<ShippingTypesController>
        [HttpGet]
        public ActionResult<ApiResponse<List<ShipingPackgingDto>>> Get()
        {
            try
            {
                var data = _shipingPackging.GetAll();

                return Ok(ApiResponse<List<ShipingPackgingDto>>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<List<ShipingPackgingDto>>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ShipingPackgingDto>>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }

        }

        // GET api/<ShippingTypesController>/5
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<ShipingPackgingDto>> Get(Guid id)
        {
            try
            {
                var data = _shipingPackging.GetById(id);

                return Ok(ApiResponse<ShipingPackgingDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<ShipingPackgingDto>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ShipingPackgingDto>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }
    }
}
