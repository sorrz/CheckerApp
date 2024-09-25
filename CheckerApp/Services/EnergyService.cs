using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckerApp.Services
{
    public class EnergyService
    {

        private readonly HttpClient _httpClient;

        // Constructor that initializes the HttpClient internally
        public EnergyService(HttpClient client)
        {
            _httpClient = client;
        }

        // This function fetches data asynchronously and parses it into a specific type T
        public async Task<EnergyPriceData?> GetDataAsync(string url)
        {
            try
            {
                // Send a GET request
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                // Ensure the response was successful
                response.EnsureSuccessStatusCode();

                // Read the content as a JSON string
                string jsonData = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON into the correct type
                var result = JsonSerializer.Deserialize<List<EnergyPrice>>(jsonData);

                // Return as an instance of EnergyPriceData
                return new EnergyPriceData { Prices = result };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching data: {ex.Message}");
                return default; // Return default in case of an error
            }
        }

    }

}

