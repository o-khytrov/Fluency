using Newtonsoft.Json;

namespace TravelBot;

public class WikipediaClient
{
    private readonly HttpClient _httpClient;

    private readonly string _url =
        "https://en.wikipedia.org/w/api.php?format=json&action=query&prop=extracts&exintro&explaintext=1&titles=";

    public WikipediaClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> QueryAsync(string query)
    {
        try
        {
            var response = await _httpClient.GetAsync(_url + query);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Result>(content);
            if (result?.query.pages.Any() ?? false)
            {
                var pageText = result.query.pages.First().Value.extract;
                if (!string.IsNullOrEmpty(pageText))
                {
                    return pageText.Substring(0, pageText.IndexOf('.') + 1);
                }
            }

            return string.Empty;
        }
        catch (Exception)
        {
            return string.Empty;
        }
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