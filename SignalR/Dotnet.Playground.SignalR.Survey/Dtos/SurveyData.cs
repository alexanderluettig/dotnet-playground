namespace Dotnet.Playground.SignalR.Survey;

public record class SurveyData
{
    public long ID { get; init; }
    public required string Name { get; init; }
    public required string Question { get; init; }
    public Dictionary<string, int> Answers { get; init; } = [];
}
