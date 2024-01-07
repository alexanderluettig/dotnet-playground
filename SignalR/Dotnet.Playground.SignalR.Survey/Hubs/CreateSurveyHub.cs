using Dotnet.Playground.SignalR.Survey.Database;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Playground.SignalR.Survey.Hubs;

public class CreateSurveyHub(SurveyContext surveyContext) : Hub
{
    private readonly SurveyContext _surveyContext = surveyContext;

    public override async Task OnConnectedAsync()
    {
        var surveyData = await _surveyContext.Surveys.ToListAsync();
        await Clients.Caller.SendAsync("TransferSurveyData", surveyData);
    }

    public async Task CreateSurvey(SurveyData surveyData)
    {
        await _surveyContext.Surveys.AddAsync(surveyData);
        await _surveyContext.SaveChangesAsync();

        var resultData = await _surveyContext.Surveys.ToListAsync();
        Console.WriteLine($"Survey created: {surveyData.Name}");
        await Clients.All.SendAsync("TransferSurveyData", resultData);
    }
}
