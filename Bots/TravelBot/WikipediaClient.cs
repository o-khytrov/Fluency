using Newtonsoft.Json;

namespace TravelBot;

public class WikipediaClient
{
    private readonly HttpClient _httpClient;

    private readonly string _url =
        "https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&explaintext=1&titles=";

    public WikipediaClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> QueryAsync(string query)
    {
        var response = await _httpClient.GetAsync(_url + query);
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Result>(content);
        if (result?.query?.pages.Any() ?? false)
        {
            return result.query.pages.First().Value.extract;
        }

        return null;
    }

    public class Result
    {
        public Query query { get; set; }
    }

    public class Query
    {
        public Dictionary<string, Page> pages { get; set; }
    }

    public class Page
    {
        public string extract { get; set; }
    }
}