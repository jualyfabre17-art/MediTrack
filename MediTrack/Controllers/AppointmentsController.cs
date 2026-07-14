using Microsoft.AspNetCore.Mvc;
using MediTrack.Application.Interfaces;
using MediTrack.Application.Dtos.Appointment;
using MediTrack.Application.Core;

namespace MediTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _appointmentService.GetAllAsync();
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _appointmentService.GetByIdAsync(id);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("patient/{patientId}")]
    public async Task<IActionResult> GetByPatient(int patientId)
    {
        var result = await _appointmentService.GetByPatientAsync(patientId);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("doctor/{doctorId}")]
    public async Task<IActionResult> GetByDoctor(int doctorId)
    {
        var result = await _appointmentService.GetByDoctorAsync(doctorId);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpGet("availability")]
    public async Task<IActionResult> CheckAvailability(
        [FromQuery] int doctorId,
        [FromQuery] DateTime dateTime,
        [FromQuery] int durationMinutes = 30)
    {
        var result = await _appointmentService.CheckAvailabilityAsync(doctorId, dateTime, durationMinutes);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AppointmentCreateDto createDto)
    {
        var result = await _appointmentService.CreateAsync(createDto);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAppointmentStatusDto statusDto)
    {
        var result = await _appointmentService.UpdateStatusAsync(id, statusDto.Status);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await _appointmentService.CancelAsync(id);
        if (!result.IsSuccess)
            return StatusCode(result.StatusCode, result);

        return Ok(result);
    }
}