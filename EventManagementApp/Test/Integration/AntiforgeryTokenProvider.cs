
using EventManagementApp.Tests.Integration;
using HtmlAgilityPack;

class AntiforgeryTokenProvider
{

    private readonly HttpClient _client;

    public AntiforgeryTokenProvider(HttpClient client)
    {
        _client = client;
    }
    public async Task<string> GetTokenFromPageAsync(string urlPath)
    {
        var response = await _client.GetAsync(urlPath);
        var content = await response.Content.ReadAsStringAsync();
        var html = new HtmlDocument();
        html.LoadHtml(content);
        var input = html.DocumentNode.SelectSingleNode("//input[@name='__RequestVerificationToken']") ?? throw new Exception("Input not found.");
        return input.GetAttributeValue("value", "");
    }


}