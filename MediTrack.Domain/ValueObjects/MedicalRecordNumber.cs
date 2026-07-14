namespace MediTrack.Domain.ValueObjects;

public class MedicalRecordNumber
{
    public string Value { get; private set; }

    public MedicalRecordNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Medical Record Number cannot be empty");
        Value = value;
    }

    public static MedicalRecordNumber Generate()
    {
        var random = new Random();
        var year = DateTime.Now.Year.ToString().Substring(2);
        var number = random.Next(10000, 99999).ToString();
        return new MedicalRecordNumber($"MR-{year}-{number}");
    }

    public override string ToString() => Value;
}
