using BlazorApp.Contracts.@base;
using Shared.Common;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorApp.Services.@base
{
    public class BaseService<DTO> : IBaseService<DTO>
    {
        private readonly HttpClient _http;
        //public string _endpoint = "/"; 
        public virtual string _endpoint { get; set; }


        public BaseService(HttpClient http)
        {
            _http = http;
        }
        

        public async Task<ApiResponse<List<DTO>>> GetAll()
        {
            try
            {
                var data = await _http.GetFromJsonAsync<ApiResponse<List<DTO>>>(_endpoint);
                return ApiResponse<List<DTO>>.SuccessResponse(data!.Data!);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<DTO>>.FailResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<DTO>> GetById(Guid id)
        {
            try
            {
                var data = await _http.GetFromJsonAsync<ApiResponse<DTO>>($"{_endpoint}/{id}");
                return ApiResponse<DTO>.SuccessResponse(data!.Data!);
            }
            catch (Exception ex)
            {
                return ApiResponse<DTO>.FailResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<DTO>> Add(DTO entity)
        {
            try
            {
                var response = await _http.PostAsJsonAsync(_endpoint, entity);
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<DTO>>();
                return result ?? ApiResponse<DTO>.FailResponse("No response from server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<DTO>.FailResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> Delete(Guid id)
        {
            try
            {
                var response = await _http.DeleteAsync($"{_endpoint}/{id}");
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                return result ?? ApiResponse<bool>.FailResponse("No response from server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.FailResponse(ex.Message);
            }
        }

        public async Task<ApiResponse<DTO>> Update(Guid id, DTO entity)
        {
            Console.WriteLine("I Will Update Now .....");
            try
            {
                var response = await _http.PutAsJsonAsync($"{_endpoint}", entity);
                
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<DTO>>();
                return result ?? ApiResponse<DTO>.FailResponse("No response from server.");
            }
            catch (Exception ex)
            {
                return ApiResponse<DTO>.FailResponse(ex.Message);
            }
        }
    }
}
