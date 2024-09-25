using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;

public class WeatherData
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public double GenerationtimeMs { get; set; }

    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; }

    [JsonPropertyName("timezone_abbreviation")]
    public string TimezoneAbbreviation { get; set; }

    [JsonPropertyName("elevation")]
    public double Elevation { get; set; }

    [JsonPropertyName("hourly_units")]
    public HourlyUnits HourlyUnits { get; set; }

    [JsonPropertyName("hourly")]
    public Hourly Hourly { get; set; }


    //Method to get the temperature and corresponding time for the current time
    public (string time, double temperature)? GetCurrentHourlyData()
    {
        // Get current time in UTC and then apply the UTC offset from the weather data
        DateTime currentTimeUtc = DateTime.UtcNow;
        DateTime currentTimeInLocation = currentTimeUtc.AddSeconds(UtcOffsetSeconds);

        // Try to match the current time (ignoring minutes) to the hourly time data
        for (int i = 0; i < Hourly.Time.Count; i++)
        {
            if (DateTime.TryParseExact(Hourly.Time[i], "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime hourlyTime))
            {
                if (hourlyTime == currentTimeInLocation.Date.AddHours(currentTimeInLocation.Hour))
                {
                    return (Hourly.Time[i], Hourly.Temperature2m[i]);
                }
            }
        }

        // Return null if no matching time is found
        return null;
    }
}

public class HourlyUnits
{
    [JsonPropertyName("time")]
    public string Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public string Temperature2m { get; set; }
}

public class Hourly
{
    [JsonPropertyName("time")]
    public List<string> Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public List<double> Temperature2m { get; set; }
}
