using System.Text.Json;
using Ukol2.Dto;

namespace Ukol2
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://swapi.dev/api/")
        };

        static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        static async Task Main(string[] args)
        {
            try
            {
                Planet kashyyyk = await GetPlanet("Kashyyyk");

                var residentTasks = new List<Task<Person>>();

                foreach (var residentUrl in kashyyyk.Residents)
                {
                    residentTasks.Add(GetPersonByUrlAsync(residentUrl));
                }

                var residents = await Task.WhenAll(residentTasks);

                var results = new List<SearchResult>();

                foreach (var resident in residents)
                {
                    foreach (var starshipUrl in resident.Starships)
                    {
                        //sekvencni stahovani, slo by to udelat paralelne jako u obyvatel....
                        var ship = await GetStarship(starshipUrl);
                        results.Add(new SearchResult(ship.Name, ship.Model, ship.StarshipClass, ship.Manufacturer, ship.CostInCredits, resident.Name));
                    }
                }

                var outputOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(results, outputOptions);
                Console.WriteLine(json);
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"Chyba při komunikaci s API: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Vyskytla se chyba: {ex.Message}");
            }
        }

        private static async Task<Starship> GetStarship(string url)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Starship>(json, options) ?? throw new Exception("Chyba při deserializaci.");
        }

        private static async Task<Planet> GetPlanet(string name)
        {
            var response = await httpClient.GetAsync($"planets/?search={name}");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var page = JsonSerializer.Deserialize<PagedResult<Planet>>(json, options) ?? throw new Exception("Chyba při deserializaci.");

            var planet = page.Results.FirstOrDefault(p => p.Name == name) ?? throw new Exception($"Planeta {name} nebyla nalezena.");

            return planet;
        }

        private static async Task<Person> GetPersonByUrlAsync(string url)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Person>(json, options) ?? throw new Exception("Chyba při deserializaci.");
        }
    }
}
