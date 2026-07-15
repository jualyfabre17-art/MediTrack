using System.Net.Http.Json;
using MediTrack.BlazorWASM.Models;

namespace MediTrack.BlazorWASM.Services;

public class PatientService : IPatientService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "api/patients";

    public PatientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PatientModel>> GetPatientsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync(BaseUrl);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<PatientModel>>>();
                return result?.Data ?? new List<PatientModel>();
            }
            return new List<PatientModel>();
        }
        catch
        {
            return new List<PatientModel>();
        }
    }

    public async Task<PatientModel?> GetPatientAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PatientModel>>();
                return result?.Data;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<PatientModel?> CreatePatientAsync(PatientModel patient)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, patient);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ApiResponse<PatientModel>>();
                return result?.Data;
            }
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> UpdatePatientAsync(PatientModel patient)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{patient.Id}", patient);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeletePatientAsync(int id)
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
