using iteam.Libo.Common;
using System.Net.Http.Json;

namespace iteam.Libo.WebApp.Services;

public class ArticleService
{
    private readonly HttpClient _httpClient;

    public ArticleService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Article>> GetArticlesAsync()
    {
        return await _httpClient.GetFromJsonAsync<List<Article>>(requestUri: "https://localhost:7112/articles");
    }
}
