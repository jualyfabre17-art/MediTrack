using System.Net.Http.Json;
using MediTrack.BlazorWASM.Models;

namespace MediTrack.BlazorWASM.Services;

public class AppointmentService : IAppointmentService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "api/appointments";

    public AppointmentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AppointmentModel>> GetAppointmentsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<AppointmentModel>>>();
                return result?.Data ?? new List<AppointmentModel>();
            }
            return new List<AppointmentModel>();
        }
        catch
        {
            return new List<AppointmentModel>();
        }
    }

    public async Task<AppointmentModel?> GetAppointmentAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<AppointmentModel>>();
                return result?.Data;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<AppointmentModel?> CreateAppointmentAsync(CreateAppointmentModel appointment)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, appointment);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<AppointmentModel>>();
                return result?.Data;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> CancelAppointmentAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
