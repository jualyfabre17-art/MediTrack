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

    public async Task<bool> UpdateDoctorAsync(DoctorUpdateModel doctor)
    {
        try
        {
            if (TimeOnly.TryParse(doctor.StartTime, out var start) && TimeOnly.TryParse(doctor.EndTime, out var end))
            {
                var payload = new
                {
                    doctor.Id,
                    doctor.FirstName,
                    doctor.LastName,
                    doctor.Email,
                    doctor.PhoneNumber,
                    doctor.DateOfBirth,
                    doctor.Gender,
                    doctor.LicenseNumber,
                    doctor.YearsOfExperience,
                    doctor.OfficeLocation,
                    doctor.ConsultationFee,
                    StartTime = start,
                    EndTime = end,
                    doctor.DepartmentId,
                    doctor.SpecialtyId,
                    Address = new
                    {
                        doctor.Address.Street,
                        doctor.Address.City,
                        doctor.Address.State,
                        doctor.Address.PostalCode,
                        doctor.Address.Country
                    }
                };

                var response = await _httpClient.PutAsJsonAsync($"{BaseUrl}/{doctor.Id}", payload);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al actualizar doctor: {response.StatusCode} - {error}");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Formato de hora inválido. Use HH:mm");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en UpdateDoctorAsync: {ex.Message}");
            return false;
        }
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

    public async Task<DoctorModel?> CreateDoctorAsync(DoctorCreateModel doctor)
    {
        try
        {
            if (TimeOnly.TryParse(doctor.StartTime, out var start) && TimeOnly.TryParse(doctor.EndTime, out var end))
            {
                var payload = new
                {
                    doctor.FirstName,
                    doctor.LastName,
                    doctor.Email,
                    doctor.PhoneNumber,
                    doctor.DateOfBirth,
                    doctor.Gender,
                    doctor.LicenseNumber,
                    doctor.YearsOfExperience,
                    doctor.OfficeLocation,
                    doctor.ConsultationFee,
                    StartTime = start,
                    EndTime = end,
                    doctor.DepartmentId,
                    doctor.SpecialtyId,
                    Address = new
                    {
                        doctor.Address.Street,
                        doctor.Address.City,
                        doctor.Address.State,
                        doctor.Address.PostalCode,
                        doctor.Address.Country
                    }
                };

                var response = await _httpClient.PostAsJsonAsync(BaseUrl, payload);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse<DoctorModel>>();
                    return result?.Data;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error al crear doctor: {response.StatusCode} - {error}");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Formato de hora inválido. Use HH:mm");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Excepción en CreateDoctorAsync: {ex.Message}");
            return null;
        }
    }
}
