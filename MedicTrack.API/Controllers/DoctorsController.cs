using Microsoft.AspNetCore.Mvc;
using MediTrack.Application.Interfaces;
using MediTrack.Application.Dtos.Doctor;
using MediTrack.Application.Core;

namespace MediTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _doctorService.GetAllAsync();
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _doctorService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("specialty/{specialtyId}")]
    public async Task<IActionResult> GetBySpecialty(int specialtyId)
    {
        var result = await _doctorService.GetBySpecialtyAsync(specialtyId);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("department/{departmentId}")]
    public async Task<IActionResult> GetByDepartment(int departmentId)
    {
        var result = await _doctorService.GetByDepartmentAsync(departmentId);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailable([FromQuery] DateTime dateTime)
    {
        var result = await _doctorService.GetAvailableDoctorsAsync(dateTime);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DoctorCreateDto createDto)
    {
        var result = await _doctorService.CreateAsync(createDto);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DoctorUpdateDto updateDto)
    {
        if (id != updateDto.Id)
            return BadRequest(ServiceResult<DoctorResponseDto>.Error("ID mismatch"));

        var result = await _doctorService.UpdateAsync(updateDto);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _doctorService.DeleteAsync(id);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }
}