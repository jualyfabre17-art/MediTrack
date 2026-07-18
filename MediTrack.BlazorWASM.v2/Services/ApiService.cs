using System.Net.Http.Json;
using MediTrack.Web.V2.Models; // Asegúrate de que apunte a tus modelos

namespace MediTrack.Web.V2.Services;

public class ApiService
{
    private readonly HttpClient _http;

    public ApiService(HttpClient http)
    {
        _http = http;
    }

    // GET: Para traer listas o elementos únicos
    public async Task<ServiceResult<T>> GetAsync<T>(string url)
    {
        try
        {
            var response = await _http.GetFromJsonAsync<ServiceResult<T>>(url);
            return response ?? new ServiceResult<T> { IsSuccess = false, Message = "No response from server." };
        }
        catch (Exception ex)
        {
            return new ServiceResult<T> { IsSuccess = false, Message = $"Request failed: {ex.Message}" };
        }
    }

    // POST: Para enviar creaciones (Citas, Pacientes, Doctores)
    public async Task<ServiceResult<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(url, data);
            var result = await response.Content.ReadFromJsonAsync<ServiceResult<TResponse>>();
            return result ?? new ServiceResult<TResponse> { IsSuccess = false, Message = "Error parsing server response." };
        }
        catch (Exception ex)
        {
            return new ServiceResult<TResponse> { IsSuccess = false, Message = $"Request failed: {ex.Message}" };
        }
    }

    // PUT: Para actualizaciones completas
    public async Task<ServiceResult<TResponse>> PutAsync<TRequest, TResponse>(string url, TRequest data)
    {
        try
        {
            var response = await _http.PutAsJsonAsync(url, data);
            var result = await response.Content.ReadFromJsonAsync<ServiceResult<TResponse>>();
            return result ?? new ServiceResult<TResponse> { IsSuccess = false, Message = "Error parsing server response." };
        }
        catch (Exception ex)
        {
            return new ServiceResult<TResponse> { IsSuccess = false, Message = $"Request failed: {ex.Message}" };
        }
    }

    // DELETE: Para inactivar registros
    public async Task<ServiceResult<bool>> DeleteAsync(string url)
    {
        try
        {
            var response = await _http.DeleteAsync(url);
            var result = await response.Content.ReadFromJsonAsync<ServiceResult<bool>>();
            return result ?? new ServiceResult<bool> { IsSuccess = false, Message = "Error parsing server response." };
        }
        catch (Exception ex)
        {
            return new ServiceResult<bool> { IsSuccess = false, Message = $"Request failed: {ex.Message}" };
        }
    }
}
