using System.Net.Http.Json;
using System.Text.Json;

namespace App.WindowsService;

public class provaServizio
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    private const string ProvaApiUrl =
        "https://karljoke.herokuapp.com/jokes/programming/random";

    public provaServizio(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<string> GetProvaAsync()
    {
        try
        {
            // The API returns an array with a single entry.
            Prova[]? prove = await _httpClient.GetFromJsonAsync<Prova[]>(
                ProvaApiUrl, _options);

            Prova? prova = prove?[0];

            return prova is not null
                ? $"{prova.Setup}{Environment.NewLine}{prova.Punchline}"
                : "niente";
        }
        catch (Exception ex)
        {
            return $"Errore {ex}";
        }
    }
}

public record Prova(int Id, string Type, string Setup, string Punchline);