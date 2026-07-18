namespace MediTrack.Web.V2.Models; // Ajusta el namespace al nombre exacto de tu proyecto frontend

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public int StatusCode { get; set; }
}
