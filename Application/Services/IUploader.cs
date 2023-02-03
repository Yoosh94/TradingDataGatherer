using Domain.Models;

namespace Application.Services;

public interface IUploader
{
    public Task Upload(List<Daily> dailyBars);
}