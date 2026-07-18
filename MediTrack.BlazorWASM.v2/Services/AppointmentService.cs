using System.Net.Http.Json;
using System.Text.Json;
using MediTrack.BlazorWASM.v2.Models;

namespace MediTrack.BlazorWASM.v2.Services;

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
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ApiResponse<List<AppointmentModel>>>(content);
                return result?.Data ?? new List<AppointmentModel>();
            }
            Console.WriteLine($"Error GET appointments: {response.StatusCode} - {content}");
            return new List<AppointmentModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en GetAppointmentsAsync: {ex.Message}");
            return new List<AppointmentModel>();
        }
    }

    public async Task<AppointmentModel?> GetAppointmentAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ApiResponse<AppointmentModel>>(content);
                return result?.Data;
            }
            Console.WriteLine($"Error GET appointment by id: {response.StatusCode} - {content}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en GetAppointmentAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<AppointmentModel?> CreateAppointmentAsync(AppointmentCreateModel appointment)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, appointment);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ApiResponse<AppointmentModel>>(content);
                return result?.Data;
            }
            Console.WriteLine($"Error POST appointment: {response.StatusCode} - {content}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en CreateAppointmentAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> CancelAppointmentAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
                return true;
            Console.WriteLine($"Error DELETE appointment: {response.StatusCode}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en CancelAppointmentAsync: {ex.Message}");
            return false;
        }
    }
}