namespace MediTrack.Application.Core;

public class ServiceResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public int StatusCode { get; set; } = 200;

    public static ServiceResult<T> Success(T data, string message = "Operation completed successfully")
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
            StatusCode = 200
        };
    }

    public static ServiceResult<T> Success(string message = "Operation completed successfully")
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Message = message,
            StatusCode = 200
        };
    }

    public static ServiceResult<T> Error(string message, int statusCode = 400)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode
        };
    }

    public static ServiceResult<T> ValidationError(List<string> errors)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            Errors = errors,
            StatusCode = 400
        };
    }

    public static ServiceResult<T> NotFound(string message = "Resource not found")
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = 404
        };
    }

    public static ServiceResult<T> Conflict(string message = "Resource conflict")
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            Message = message,
            StatusCode = 409
        };
    }
}
