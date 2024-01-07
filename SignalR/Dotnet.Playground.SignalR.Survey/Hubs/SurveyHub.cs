using Dotnet.Playground.SignalR.Survey.Database;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Dotnet.Playground.SignalR.Survey.Hubs;

public class SurveyHub : Hub
{
    private readonly SurveyContext _surveyContext;

    public SurveyHub(SurveyContext surveyContext)
    {
        _surveyContext = surveyContext;
    }

    public async Task FetchData(string surveyName)
    {
        var surveyData = await _surveyContext.Surveys.FirstOrDefaultAsync(s => s.Name == surveyName) ?? throw new Exception("Survey not found");
        await Clients.Caller.SendAsync("ReceiveSurveyData", surveyData);
    }

    public async Task UpdateAnswer(string surveyName, string answer)
    {
        var surveyData = await _surveyContext.Surveys.FirstOrDefaultAsync(s => s.Name == surveyName) ?? throw new Exception("Survey not found");
        surveyData.Answers[answer] = surveyData.Answers[answer] + 1;
        await _surveyContext.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveSurveyData", surveyData);
    }
}
