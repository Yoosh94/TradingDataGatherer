using Domain.Models;

namespace Application.Services;

public interface IRepository
{
    public Task<List<Daily>> GetDailyBarsAsync(string symbol, DateTime from, DateTime to);
}