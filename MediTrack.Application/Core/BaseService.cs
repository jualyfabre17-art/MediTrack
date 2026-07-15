using MediTrack.Domain.Interfaces;
using MediTrack.Application.Core;

namespace MediTrack.Application.Core;

public abstract class BaseService
{
    protected readonly IUnitOfWork _unitOfWork;

    protected BaseService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected ServiceResult<T> HandleError<T>(string message, int statusCode = 400)
    {
        return ServiceResult<T>.Error(message, statusCode);
    }

    protected ServiceResult<T> HandleNotFound<T>(string message = "Resource not found")
    {
        return ServiceResult<T>.NotFound(message);
    }

    protected ServiceResult<T> HandleConflict<T>(string message = "Resource conflict")
    {
        return ServiceResult<T>.Conflict(message);
    }
}
