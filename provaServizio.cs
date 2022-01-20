using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace App.WindowsService
{
    /*public class provaServizio
    {
        private readonly IConfiguration Configuration;
        private readonly HttpClient _httpClient;
        public class EmailConfig
        {
            public string? Address { get; set; }
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        private const string ProvaApiUrl = "https://karljoke.herokuapp.com/jokes/programming/random";
        public provaServizio(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<EmailConfig> GetProvaAsync(EmailConfig emailConfig, int ciclo)
        {
            //try{
            Console.WriteLine("run #" + ciclo);
            Console.WriteLine(emailConfig.Address);
            Console.WriteLine(emailConfig.Username);
            Console.WriteLine(emailConfig.Password);

            // The API returns an array with a single entry. //
            //Prova[]? prove = await _httpClient.GetFromJsonAsync<Prova[]>(ProvaApiUrl);
            //Prova? prova = prove?[0];

            EmailConfig nextEmailConfig = new EmailConfig
            {
                Address = "address #" + ciclo.ToString(),
                Username = "username #" + ciclo.ToString(),
                Password = "password #" + ciclo.ToString()
            };

            return nextEmailConfig;
            //}
            catch (Exception ex)
            {
                return null;
            }
        }
    }
    public record Prova(int Id, string Type, string Setup, string Punchline);*/
}