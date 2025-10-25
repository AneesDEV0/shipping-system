using BusinessLayer.Contracts;
using DataAccess.Exceptions;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.Dtos;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class CitiesController : ControllerBase
    {
        ICityService _city;
        public CitiesController(ICityService city)
        {
            _city = city;
        }
        // GET: api/<cityController>
        [HttpGet]
        public ActionResult<ApiResponse<List<CityDto>>> Get()
        {
            try
            {
                var data = _city.GetAllCitites();

                return Ok(ApiResponse<List<CityDto>>.SuccessResponse(data));
            }
            catch(DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<List<CityDto>>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ApiResponse<List<CityDto>>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }

        }

        // GET api/<cityController>/5
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<CityDto>> Get(Guid id)
            {
            try
            {
                var data = _city.GetById(id);

                return Ok(ApiResponse<CityDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<CityDto>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CityDto>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }

        [HttpGet("GetByCountry/{id}")]
        public ActionResult<ApiResponse<CityDto>> GetByCountry(Guid id)
        {
            try
            {
                var data = _city.GetByCountry(id);

                return Ok(ApiResponse<List<CityDto>>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<List<CityDto>>.FailResponse
                    ("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<CityDto>>.FailResponse
                    ("general exception", new List<string>() { ex.Message }));
            }
        }

        // POST: api/Cities
        [HttpPost]
        public ActionResult<ApiResponse<CountryDto>> Post([FromBody] CityDto dto)
        {
            try
            {
                var result = _city.Add(dto, out Guid id);
                if (result)
                {
                    dto.Id = id; // عشان يرجع الـ Id الجديد
                    return Ok(ApiResponse<CityDto>.SuccessResponse(dto, "Added successfully"));
                }
                return BadRequest(ApiResponse<CityDto>.FailResponse("Failed to add"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    ApiResponse<CountryDto>.FailResponse("general exception", new List<string>() { ex.Message }));
            }
        }


    }
}
