// See https://aka.ms/new-console-template for more information

using Application;
using Application.Services;
using Domain.Models;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = CreateHostBuilder(args).Build();

var ruleEngine = host.Services.GetRequiredService<IRuleEngine>();
var uploader = host.Services.GetRequiredService<IUploader>();
Console.WriteLine("Welcome to the data gatherer.");

Console.WriteLine("Enter the stock you want to get data for:");
var symbol = Console.ReadLine();

Console.WriteLine("Enter the date of when you want to get data from in dd mmm YYYY format");
var startDateInput = Console.ReadLine();
DateTime startDate;
var startDateCorrect = DateTime.TryParse(startDateInput, out startDate);
if (!startDateCorrect) throw new Exception("Incorrect date");

Console.WriteLine("Enter the date of when you want to get data to in dd mmm YYYY format");
var endDateInput = Console.ReadLine();
DateTime endDate;
var endDateCorrect = DateTime.TryParse(endDateInput, out endDate);
if (!endDateCorrect) throw new Exception("Incorrect date");

await ruleEngine.Activate(symbol!, startDate, endDate);

Console.WriteLine("What daily candles are you looking for?");
Console.WriteLine("1. Opening outside the body of the previous day");
Console.WriteLine("2. Opening inside the previous day");
Console.WriteLine("3. Bearish retest gap"); 
Console.WriteLine("4. Bearish gap and go"); 
Console.WriteLine("5. Bullish retest gap"); 
Console.WriteLine("6. Bullish gap and go"); 
Console.WriteLine("7. All Candles");

var option = Console.ReadLine();
var opt = int.Parse(option);
List<Daily> dataToUpload = new List<Daily>();
switch (opt)
{
    case 1:
        dataToUpload = ruleEngine.OpenOutsideOfBody().Execute();
        break;
    case 3:
        dataToUpload = ruleEngine.OpenBelowYesterdayLow().YesterdayClosedRed().Execute();
        break;
    case 4:
        dataToUpload = ruleEngine.OpenBelowYesterdayLow().YesterdayClosedGreen().Execute();
        break;
    case 5:
        dataToUpload = ruleEngine.OpenAboveYesterdayHigh().YesterdayClosedGreen().Execute();
        break;
    case 6:
        dataToUpload = ruleEngine.OpenAboveYesterdayHigh().YesterdayClosedRed().Execute();
        break;
    case 7:
        dataToUpload = ruleEngine.AllCandles().Execute();
        break;
    
}

await uploader.Upload(dataToUpload);

static IHostBuilder CreateHostBuilder(string[] args)
{
    var hostBuilder = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, builder) =>
        {
            builder.SetBasePath(Directory.GetCurrentDirectory());
        })
        .ConfigureServices((context, services) =>
        {
            services.AddApplicationServices();
            services.AddInfrastructureServices();
        });

    return hostBuilder;
} 