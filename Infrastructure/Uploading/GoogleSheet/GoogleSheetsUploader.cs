using Application.Services;
using Domain.Models;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Infrastructure.Uploading.GoogleSheet;

public class GoogleSheetsUploader : IUploader
{
    private readonly SheetsService _googleSheetClient;
    // private readonly Spreadsheet _spreadsheet;
    //private readonly string _spreadSheetId = "1agYxgcuas1LL_LWPrLpNE5fzalyNMLSnbvpFbLSA5cI";
    private readonly string _spreadSheetId = "1-vPddoBSidX1i_qIWwaBTWpNrAuMug-mbhV5b_aE2JQ"; // This is the id for the playbook.

    public GoogleSheetsUploader(SheetsService googleSheetClient)
    {
        _googleSheetClient = googleSheetClient;
        // _spreadsheet = _googleSheetClient.Spreadsheets.Get("1agYxgcuas1LL_LWPrLpNE5fzalyNMLSnbvpFbLSA5cI").Execute();

        // Create sheet
        // BatchUpdateSpreadsheetRequest request = new BatchUpdateSpreadsheetRequest()
        // {
        //     Requests = new List<Request>()
        //     {
        //         new Request()
        //         {
        //             AddSheet = new AddSheetRequest()
        //             {
        //                 Properties = new SheetProperties()
        //                 {
        //                     Title = "AUTOMATED",
        //                     Index = 1,
        //                 }
        //             }
        //         }
        //     }
        // };
        
    }
    public Task Upload(List<Daily> dailyBars)
    {
        // https://code-maze.com/google-sheets-api-with-net-core/
        //var dataToUpload = GoogleSheetMapper.MapToRangeData(dailyBars);
        var dataToUpload = GoogleSheetMapper.MapToPlayBookData(dailyBars);

        var valueRange = new ValueRange()
        {
            Values = dataToUpload,
        };
        var appendRequest =
            _googleSheetClient.Spreadsheets.Values.Update(valueRange, _spreadSheetId, "SymbolImporter");
        appendRequest.ValueInputOption =
            SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        appendRequest.Execute();
        return Task.CompletedTask;
    }
}