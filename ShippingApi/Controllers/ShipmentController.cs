using BusinessLayer.Contracts;
using Shared.Dtos;
using DataAccess.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;


namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        IShipment _shipment;
        IUserService _userService;
        public ShipmentController(IShipment shipment, IUserService userService)
        {
            _shipment = shipment;
            _userService = userService;
        }


        // GET: api/<ShipmentController>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ShipmentDto>>>> Get()
        {
            try
            {
                var data = await _shipment.GetShipments(); // أو _repo.GetShipments() حسب مكان التنفيذ

                return Ok(ApiResponse<List<ShipmentDto>>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<List<ShipmentDto>>.FailResponse(
                    "data access exception",
                    new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<ShipmentDto>>.FailResponse(
                    "general exception",
                    new List<string> { ex.Message }));
            }
        }

        // GET api/<ShipmentController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ShipmentDto>>> Get(Guid id)
        {
            try
            {
                var data = await _shipment.GetShipment(id); // أو _repo.GetShipments() حسب مكان التنفيذ

                return Ok(ApiResponse<ShipmentDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<ShipmentDto>.FailResponse(
                    "data access exception",
                    new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ShipmentDto>.FailResponse(
                    "general exception",
                    new List<string> { ex.Message }));
            }
        }



        [HttpPost("Create")]
        public IActionResult Create([FromBody] ShipmentDto data)
        {
            if (data == null)
            {
                return BadRequest(ApiResponse<string>.FailResponse("Shipment data is required."));
            }

            try
            {
                var result = _shipment.Create(data);    

                return Ok(ApiResponse<object>.SuccessResponse(result, "Shipment created successfully."));
            }
            catch (Exception ex)
            {
                var errors = new List<string> { ex.Message };
                return StatusCode(500, ApiResponse<string>.FailResponse("An error occurred while creating the shipment.", errors));
            }
        }

        [HttpPost("Edit")]
        public IActionResult Edit([FromBody] ShipmentDto data)
        {
            if (data == null)
            {
                return BadRequest(ApiResponse<string>.FailResponse("Shipment data is required."));
            }

            try
            {
                var result = _shipment.Edit(data);

                return Ok(ApiResponse<object>.SuccessResponse(result, "Shipment created successfully."));
            }
            catch (Exception ex)
            {
                var errors = new List<string> { ex.Message };
                return StatusCode(500, ApiResponse<string>.FailResponse("An error occurred while creating the shipment.", errors));
            }
        }

    }
}
