namespace Dotnet.Playground.SignalR.Survey;

public record class SurveyData
{
    public required string Name { get; init; }
    public required string Question { get; init; }
    public required IDictionary<string, int> Answers { get; init; }
}
