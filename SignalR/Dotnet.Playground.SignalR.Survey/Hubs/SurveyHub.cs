using Microsoft.AspNetCore.SignalR;

namespace Dotnet.Playground.SignalR.Survey.Hubs;

public class SurveyHub : Hub
{
    private IEnumerable<SurveyData> _surveyData = [ new SurveyData
    {
        Name = "Survey 1",
        Question = "What is your favorite color?",
        Answers = new Dictionary<string, int>
        {
            { "Red", 0 },
            { "Blue", 0 },
            { "Green", 0 },
            { "Yellow", 0 },
            { "Orange", 0 },
            { "Purple", 0 },
            { "Black", 0 },
            { "White", 0 },
            { "Pink", 0 },
            { "Brown", 0 },
        }
    }];

    public override async Task OnConnectedAsync()
    {
        await Clients.Caller.SendAsync("TransferSurveyData", _surveyData);
    }

    public async Task CreateSurvey(SurveyData surveyData)
    {
        _surveyData = _surveyData.Append(surveyData);
        Console.WriteLine($"Survey created: {surveyData.Name}");
        await Clients.All.SendAsync("TransferSurveyData", _surveyData);
    }
}
