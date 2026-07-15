using System.Net.Http.Json;
using MediTrack.BlazorWASM.Models;

namespace MediTrack.BlazorWASM.Services;

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
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<DoctorModel>>>();
                return result?.Data ?? new List<DoctorModel>();
            }
            return new List<DoctorModel>();
        }
        catch
        {
            return new List<DoctorModel>();
        }
    }

    public async Task<DoctorModel?> GetDoctorAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<DoctorModel>>();
                return result?.Data;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }
}
