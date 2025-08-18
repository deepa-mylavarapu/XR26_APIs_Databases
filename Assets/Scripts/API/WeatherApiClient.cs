using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using WeatherApp.Data;

namespace WeatherApp.Services
{
    public class WeatherApiClient : MonoBehaviour
    {
        [SerializeField] private string apiKey = "YOUR_API_KEY";
        [SerializeField] private string baseUrl = "https://api.openweathermap.org/data/2.5/weather";

        /// <summary>
        /// Fetches weather data for a given city using UnityWebRequest and async/await.
        /// Handles network, protocol, and data processing errors explicitly.
        /// </summary>
        public async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(city))
            {
                Debug.LogError("City name cannot be empty");
                return null;
            }

            string url = $"{baseUrl}?q={city}&appid={apiKey}";

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                await request.SendWebRequest();

                // Handle different error types specifically
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        return ParseWeatherData(request.downloadHandler.text);

                    case UnityWebRequest.Result.ConnectionError:
                        Debug.LogError($"Network connection failed: {request.error}");
                        break;

                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError($"HTTP Error {request.responseCode}: {request.error}");
                        break;

                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError($"Data processing failed: {request.error}");
                        break;
                }

                return null;
            }
        }

        /// <summary>
        /// Parses JSON string into WeatherData using safe settings.
        /// </summary>
        private WeatherData ParseWeatherData(string jsonString)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };

                // Fixed: Use the generic method to deserialize into WeatherData
                return JsonConvert.DeserializeObject<WeatherData>(jsonString, settings);
            }
            catch (JsonException ex)
            {
                Debug.LogError($"JSON parsing failed: {ex.Message}");
                return null;
            }
        }
    }
}

public class ApiClient : MonoBehaviour
{
   

    [SerializeField] private string apiKey = "YOUR_API_KEY";
    [SerializeField] private string baseUrl = "https://api.openweathermap.org/data/2.5/weather";

    /// <summary>
    /// Fetches weather data for a given city using UnityWebRequest and async/await.
    /// Handles network, protocol, and data processing errors explicitly.
    /// </summary>
    public async Task<WeatherData> GetWeatherDataAsync(string city)
    {
        // Input validation
        if (string.IsNullOrWhiteSpace(city))
        {
            Debug.LogError("City name cannot be empty");
            return null;
        }

        string url = $"{baseUrl}?q={city}&appid={apiKey}";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            await request.SendWebRequest();

            // Handle different error types specifically
            switch (request.result)
            {
                case UnityWebRequest.Result.Success:
                    return ParseWeatherData(request.downloadHandler.text);

                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError($"Network connection failed: {request.error}");
                    break;

                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"HTTP Error {request.responseCode}: {request.error}");
                    break;

                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"Data processing failed: {request.error}");
                    break;
            }

            return null;
        }
    }

    /// <summary>
    /// Parses JSON string into WeatherData using safe settings.
    /// </summary>
    private WeatherData ParseWeatherData(string json)
    {
        try
        {
            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.DeserializeObject<WeatherData>(json, settings);
        }
        catch (JsonException ex)
        {
            Debug.LogError($"JSON parsing failed: {ex.Message}");
            return null;

        }
    }


}