using System.Text.Json;
using HtmlAgilityPack;

const string mainUrl = "https://en.wikipedia.org";
const string startingPage = "/wiki/ISO_3166-2";
var html = await GetHtmlAsync(mainUrl + startingPage);

var countryDictionary = new Dictionary<string, Country>();

foreach (var country in await ExtractCountries(mainUrl + startingPage))
{
    Console.WriteLine($"Extracting {country.country} ({country.shorthandle})");

    var states = country.hasStates ? await ExtractStates(mainUrl + country.url) : new List<(string stateHandle, string stateName)>();
    countryDictionary.Add(country.shorthandle, new Country
    {
        Shorthandle = country.shorthandle,
        Name = country.country,
        Url = country.url,
        States = states.Select(state => new State
        {
            Shorthandle = state.stateHandle,
            Name = state.stateName
        })
    });

    await Task.Delay(100);
}

var json = JsonSerializer.Serialize(countryDictionary, new JsonSerializerOptions
{
    WriteIndented = true
});

File.WriteAllText("countries.json", json);

Console.WriteLine(json);

async Task<HtmlDocument> GetHtmlAsync(string url)
{
    var httpClient = new HttpClient();
    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html");
    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/85.0.4183.102 Safari/537.36");
    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "de-DE");
    var html = await httpClient.GetStringAsync(url);

    HtmlDocument htmlSnippet = new();
    htmlSnippet.LoadHtml(html);

    return htmlSnippet;
}

async Task<IEnumerable<(string shorthandle, string country, string url, bool hasStates)>> ExtractCountries(string crawlUrl)
{
    var html = await GetHtmlAsync(crawlUrl);
    var countries = new List<(string shorthandle, string country, string url, bool hasStates)>();

    //table[@class='wikitable sortable']/tbody/tr/td[2]/a
    var countryNodes = html
        .DocumentNode
        .SelectNodes("//table")
        .Where(node => node.HasClass("wikitable") && node.HasClass("sortable"))
        .SelectMany(node => node.SelectNodes("tbody/tr").Skip(1));

    foreach (var countryNode in countryNodes)
    {
        var dataNodes = countryNode.Descendants("td");
        var shorthandle = dataNodes.ElementAt(0).InnerText;
        var countryName = dataNodes.ElementAt(1).InnerText;
        var url = dataNodes.ElementAt(0).Descendants("a").First().GetAttributeValue("href", string.Empty);
        var hasStates = !dataNodes.ElementAt(2).InnerText.Contains("—");

        countries.Add((shorthandle, countryName, url, hasStates));
    }

    return countries;
}

async Task<IEnumerable<(string stateHandle, string stateName)>> ExtractStates(string crawlUrl)
{
    var html = await GetHtmlAsync(crawlUrl);
    var states = new List<(string stateHandle, string stateName)>();

    //table[@class='wikitable sortable']/tbody/tr/td[2]/a
    var stateNodes = html
        .DocumentNode
        .SelectNodes("//table")
        .Where(node => node.HasClass("wikitable") && node.HasClass("sortable"))
        .SelectMany(node => node.SelectNodes("tbody/tr").Skip(1));

    foreach (var stateNode in stateNodes)
    {
        var dataNodes = stateNode.Descendants("td");
        if (dataNodes.Count() < 2)
        {
            continue;
        }

        var stateHandle = dataNodes.ElementAt(0).InnerText;
        var stateName = dataNodes.ElementAt(1).InnerText;

        states.Add((stateHandle, stateName));
    }

    return states;
}

class Country
{
    public required string Shorthandle { get; set; }
    public required string Name { get; set; }
    public required string Url { get; set; }
    public IEnumerable<State> States { get; set; } = new List<State>();
}

class State
{
    public required string Shorthandle { get; set; }
    public required string Name { get; set; }
}