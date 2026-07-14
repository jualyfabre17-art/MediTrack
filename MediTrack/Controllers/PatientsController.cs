using MediTrack.Application.Core;
using MediTrack.Application.Dtos.Patient;
using MediTrack.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _patientService.GetAllAsync();
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _patientService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("medicalrecord/{medicalRecordNumber}")]
    public async Task<IActionResult> GetByMedicalRecordNumber(string medicalRecordNumber)
    {
        var result = await _patientService.GetByMedicalRecordNumberAsync(medicalRecordNumber);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string searchTerm)
    {
        var result = await _patientService.SearchAsync(searchTerm);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PatientCreateDto createDto)
    {
        var result = await _patientService.CreateAsync(createDto);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest(ServiceResult<PatientResponseDto>.Error("ID mismatch"));

        var result = await _patientService.UpdateAsync(updateDto);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _patientService.DeleteAsync(id);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }
}
