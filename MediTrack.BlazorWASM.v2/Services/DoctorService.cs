using System.Net.Http.Json;
using System.Text.Json;
using MediTrack.BlazorWASM.v2.Models;

namespace MediTrack.BlazorWASM.v2.Services;

public class DoctorService : IDoctorService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "api/doctors";

    public DoctorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<DoctorModel>> GetDoctorsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ApiResponse<List<DoctorModel>>>(content);
                return result?.Data ?? new List<DoctorModel>();
            }
            Console.WriteLine($"Error GET doctors: {response.StatusCode} - {content}");
            return new List<DoctorModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en GetDoctorsAsync: {ex.Message}");
            return new List<DoctorModel>();
        }
    }

    public async Task<DoctorModel?> GetDoctorAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ApiResponse<DoctorModel>>(content);
                return result?.Data;
            }
            Console.WriteLine($"Error GET doctor by id: {response.StatusCode} - {content}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en GetDoctorAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<DoctorModel?> CreateDoctorAsync(DoctorCreateModel doctor)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, doctor);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ApiResponse<DoctorModel>>(content);
                return result?.Data;
            }
            Console.WriteLine($"Error POST doctor: {response.StatusCode} - {content}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en CreateDoctorAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdateDoctorAsync(DoctorModel doctor)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{doctor.Id}", doctor);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return true;
            Console.WriteLine($"Error PUT doctor: {response.StatusCode} - {content}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en UpdateDoctorAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteDoctorAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
                return true;
            Console.WriteLine($"Error DELETE doctor: {response.StatusCode}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en DeleteDoctorAsync: {ex.Message}");
            return false;
        }
    }
}
