using System.Net;
using MediTrack.Domain.ValueObjects;

namespace MediTrack.Domain.Core;

public abstract class Person : BaseEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Address Address { get; set; } = new();
    public string Gender { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public int GetAge()
    {
        var today = DateTime.Today;
        var age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}