using CheckerApp.Services;
using System.Globalization;

namespace CheckerApp
{
    public class App
    {
        const string EnergyPriceLookupURL = "https://www.elprisetjustnu.se/api/v1/prices/2024/09-24_SE3.json";
        const string WeatherLookupURL = "https://api.open-meteo.com/v1/forecast?latitude=58.41086&longitude=15.62157&hourly=temperature_2m&timezone=Europe%2FBerlin&forecast_days=1";

        private readonly HttpClient _httpClient;


        public App()
        {
            _httpClient = new HttpClient();
        }

        public async Task Run()
        {

            string abc = "abc";

            



            var eneryService = new EnergyService(_httpClient);
            var weatherService = new WeatherService(_httpClient);

            var energyData = await HandleEnergyAsyncCalls(eneryService);
            var weatherData = await HandleWeatherAsyncCalls(weatherService);
            var currentData = weatherData.GetCurrentHourlyData();

            if (currentData.HasValue)
            {
                var (time, temp) = currentData.Value;

                // Parse the time string and format it as needed
                DateTime parsedTime = DateTime.ParseExact(time, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
                string formattedTime = parsedTime.ToString("yyyy-MM-dd HH:mm");

                Console.WriteLine($"Weather at {formattedTime} is {temp}°C");
            }
            else
            {
                Console.WriteLine("No data available for the current time.");
            }


            if (energyData != null)
            {
                var currentEnergyPrice = energyData.GetCurrentEnergyPrice();
                if (currentEnergyPrice != null)
                {
                    Console.WriteLine($"Current Energy Price: {currentEnergyPrice.SEK_per_kWh} SEK/kWh from {currentEnergyPrice.TimeStart} to {currentEnergyPrice.TimeEnd}");
                }
                else
                {
                    Console.WriteLine("No current energy price available.");
                }
            }

            Console.WriteLine("Press any key to quit!");
            Console.ReadLine();
        }

        private static async Task<WeatherData> HandleWeatherAsyncCalls(WeatherService weatherService)
        {
            return await weatherService.GetDataAsync($"{WeatherLookupURL}");
        }
        private static async Task<EnergyPriceData> HandleEnergyAsyncCalls(EnergyService eneryservice)
        {
            return await eneryservice.GetDataAsync($"{EnergyPriceLookupURL}");
        }
    }

}