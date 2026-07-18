using System.Net.Http.Json;
using System.Text.Json;
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
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Citas - Respuesta cruda: {content}");

            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = JsonSerializer.Deserialize<ApiResponse<List<AppointmentModel>>>(content, options);
                

                Console.WriteLine($"¿Data es null? {result?.Data == null}");
                Console.WriteLine($"Cantidad de citas: {result?.Data?.Count ?? 0}");
                return result?.Data ?? new List<AppointmentModel>();
            }
            else
            {
                Console.WriteLine($"Error HTTP {response.StatusCode}: {content}");
                return new List<AppointmentModel>();
            }
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
