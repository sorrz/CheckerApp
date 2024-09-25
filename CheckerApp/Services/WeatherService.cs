using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CheckerApp.Services
{
    public class WeatherService
    {

        private readonly HttpClient _httpClient;

        // Constructor that initializes the HttpClient internally
        public WeatherService(HttpClient client)
        {
            _httpClient = client;
        }

        // This function fetches data asynchronously and parses it into a specific type T
        public async Task<WeatherData?> GetDataAsync(string url)
        {
            try
            {
                // Send a GET request
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                // Ensure the response was successful
                response.EnsureSuccessStatusCode();

                // Read the content as a JSON string
                string jsonData = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON into the given type
                WeatherData? result = JsonSerializer.Deserialize<WeatherData>(jsonData);
                if (result != null)
                {

                    var now = DateTime.Now; // Get current time
                    foreach (var timeString in result.Hourly.Time) // Iterate through each time in Hourly.Time
                    {
                        // Parse each time string into DateTime
                        if (DateTime.TryParseExact(timeString, "yyyy-MM-ddTHH:mm",
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.AssumeUniversal,
                                                   out DateTime hourlyTime))
                        {
                            // Check if the hour matches the current hour
                            if (hourlyTime.Hour == now.Hour && hourlyTime.Date == now.Date)
                            {
                                // Get the corresponding temperature for the same index
                                int index = result.Hourly.Time.IndexOf(timeString);
                                double temperature = result.Hourly.Temperature2m[index];

                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to parse time string: {timeString}");
                        }
                    }

                }
                    return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching data: {ex.Message}");
                return default; // Return default in case of an error
            }
        }
    }

}

