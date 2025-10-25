using BusinessLayer.Services;
using Shared.Dtos;
using DataAccess.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Domains;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly IBaseService<TbCountry, CountryDto> _country;

        public CountriesController(IBaseService<TbCountry, CountryDto> country)
        {
            _country = country;
        }

        // GET: api/countries
        [HttpGet]
        public ActionResult<ApiResponse<List<CountryDto>>> Get()
        {
            try
            {
                var data = _country.GetAll();
                return Ok(ApiResponse<List<CountryDto>>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500,
                    ApiResponse<List<CountryDto>>.FailResponse("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    ApiResponse<List<CountryDto>>.FailResponse("general exception", new List<string>() { ex.Message }));
            }
        }

        // GET: api/countries/{id}
        [HttpGet("{id}")]
        public ActionResult<ApiResponse<CountryDto>> Get(Guid id)
        {
            try
            {
                var data = _country.GetById(id);
                return Ok(ApiResponse<CountryDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500,
                    ApiResponse<CountryDto>.FailResponse("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    ApiResponse<CountryDto>.FailResponse("general exception", new List<string>() { ex.Message }));
            }
        }

        // POST: api/countries
        [HttpPost]
        public ActionResult<ApiResponse<CountryDto>> Post([FromBody] CountryDto dto)
        {
            try
            {
                var result = _country.Add(dto, out Guid id);
                if (result)
                {
                    dto.Id = id; // عشان يرجع الـ Id الجديد
                    return Ok(ApiResponse<CountryDto>.SuccessResponse(dto, "Added successfully"));
                }
                return BadRequest(ApiResponse<CountryDto>.FailResponse("Failed to add"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    ApiResponse<CountryDto>.FailResponse("general exception", new List<string>() { ex.Message }));
            }
        }

        // PUT: api/countries
        [HttpPut]
        public ActionResult<ApiResponse<CountryDto>> Put([FromBody] CountryDto dto)
        {
            try
            {
                var result = _country.Update(dto);
                if (result)
                {
                    return Ok(ApiResponse<CountryDto>.SuccessResponse(dto, "Updated successfully"));
                }
                return BadRequest(ApiResponse<CountryDto>.FailResponse("Failed to update"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    ApiResponse<CountryDto>.FailResponse("general exception", new List<string>() { ex.Message }));
            }
        }

        // DELETE: api/countries/{id}
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<bool>> Delete(Guid id)
        {
            try
            {
                var result = _country.ChangeStatus(id, 0);
                if (result)
                {
                    return Ok(ApiResponse<bool>.SuccessResponse(true, "Deleted successfully"));
                }
                return BadRequest(ApiResponse<bool>.FailResponse("Failed to delete"));
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    ApiResponse<bool>.FailResponse("general exception", new List<string>() { ex.Message }));
            }
        }
    }
}
