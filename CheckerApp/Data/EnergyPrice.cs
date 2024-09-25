using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

public class EnergyPriceData
{
    public List<EnergyPrice> Prices { get; set; } // This should be a list of prices.
    public EnergyPrice? GetCurrentEnergyPrice()
    {
        DateTime currentTime = DateTime.UtcNow; // Assuming time in UTC
        foreach (var price in Prices)
        {
            if (DateTime.TryParse(price.TimeStart, null, DateTimeStyles.RoundtripKind, out DateTime startTime) &&
                DateTime.TryParse(price.TimeEnd, null, DateTimeStyles.RoundtripKind, out DateTime endTime))
            {
                // Check if current time is within the start and end time
                if (currentTime >= startTime && currentTime < endTime)
                {
                    return price; // Return the current price entry
                }
            }
        }

        // Return null if no matching time is found
        return null;
    }

}

public class EnergyPrice
{
    [JsonPropertyName("SEK_per_kWh")]
    public double SEK_per_kWh { get; set; }

    [JsonPropertyName("EUR_per_kWh")]
    public double EUR_per_kWh { get; set; }

    [JsonPropertyName("EXR")]
    public double EXR { get; set; }

    [JsonPropertyName("time_start")]
    public string TimeStart { get; set; }

    [JsonPropertyName("time_end")]
    public string TimeEnd { get; set; }

    // Override ToString method
    public override string ToString()
    {
        return $"Price: {SEK_per_kWh} SEK/kWh, {EUR_per_kWh} EUR/kWh, Start Time: {GetFormattedTimeStart()}, End Time: {GetFormattedTimeEnd()}";
    }

    public string GetFormattedTimeStart()
    {
        DateTime parsedTimeStart = DateTime.Parse(TimeStart, null, System.Globalization.DateTimeStyles.RoundtripKind);
        return parsedTimeStart.ToString("yyyy-MM-dd HH:mm");
    }

    public string GetFormattedTimeEnd()
    {
        DateTime parsedTimeEnd = DateTime.Parse(TimeEnd, null, System.Globalization.DateTimeStyles.RoundtripKind);
        return parsedTimeEnd.ToString("yyyy-MM-dd HH:mm");
    }

}
