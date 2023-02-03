using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class IoC
{
    public static void AddApplicationServices(this IServiceCollection service)
    {
        service.AddScoped<IRuleEngine, RulesEngine>();
        service.AddScoped<ICandleFilteringService,CandleFilteringService>();

    }
}