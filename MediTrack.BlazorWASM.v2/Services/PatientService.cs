using System.Net.Http.Json;
using System.Text.Json;
using MediTrack.BlazorWASM.v2.Models;

namespace MediTrack.BlazorWASM.v2.Services;

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
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ApiResponse<List<PatientModel>>>(content);
                return result?.Data ?? new List<PatientModel>();
            }
            Console.WriteLine($"Error GET patients: {response.StatusCode} - {content}");
            return new List<PatientModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en GetPatientsAsync: {ex.Message}");
            return new List<PatientModel>();
        }
    }

    public async Task<PatientModel?> GetPatientAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ApiResponse<PatientModel>>(content);
                return result?.Data;
            }
            Console.WriteLine($"Error GET patient by id: {response.StatusCode} - {content}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en GetPatientAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<PatientModel?> CreatePatientAsync(PatientCreateModel patient)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(BaseUrl, patient);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ApiResponse<PatientModel>>(content);
                return result?.Data;
            }
            Console.WriteLine($"Error POST patient: {response.StatusCode} - {content}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en CreatePatientAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> UpdatePatientAsync(PatientModel patient)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{patient.Id}", patient);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return true;
            Console.WriteLine($"Error PUT patient: {response.StatusCode} - {content}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en UpdatePatientAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeletePatientAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
            if (response.IsSuccessStatusCode)
                return true;
            Console.WriteLine($"Error DELETE patient: {response.StatusCode}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en DeletePatientAsync: {ex.Message}");
            return false;
        }
    }
}