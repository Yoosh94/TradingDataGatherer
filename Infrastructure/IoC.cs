using Application.Services;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Infrastructure.Repositories.Alpaca;
using Infrastructure.Uploading.GoogleSheet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure;

public static class IoC
{
    public static void AddInfrastructureServices(this IServiceCollection service)
    {
        service.AddHttpClient<IRepository, AlpacaRepository>(client =>
        {
            var apiKey = "AKZ00YR9LH1IR1N1Q7YU";
            var secretKey = Environment.GetEnvironmentVariable("DOTNET_Alpaca__SecretKey");
            Uri baseUrl = new Uri("https://data.alpaca.markets");
            client.BaseAddress = baseUrl;
            client.DefaultRequestHeaders.Add("APCA-API-KEY-ID",apiKey);
            client.DefaultRequestHeaders.Add("APCA-API-SECRET-KEY",secretKey);
        });

        service.AddSingleton<IUploader>(s =>
        {
            var scope = new List<string> {SheetsService.Scope.Drive};
            var googleCredentials = GoogleCredential.FromJson(Environment.GetEnvironmentVariable("DOTNET_GoogleApiCredentials"));
            var sheetService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredentials
            });
            return new GoogleSheetsUploader(sheetService);
        });
    }
}