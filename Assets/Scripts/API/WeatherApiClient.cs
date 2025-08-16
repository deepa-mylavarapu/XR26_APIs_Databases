using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using WeatherApp.Data;
using WeatherApp.Config;

namespace WeatherApp.Services
{
    /// <summary>
    /// Modern API client for fetching weather data
    /// Students will complete the implementation following async/await patterns
    /// </summary>
    public class WeatherApiClient : MonoBehaviour
    {
        [Header("API Configuration")]
        [SerializeField] private string baseUrl = "https://api.openweathermap.org/data/2.5/weather";


        /// <summary>
        /// Fetch weather data for a specific city using async/await pattern
        /// </summary>
        /// <param name="city">City name to get weather for</param>
        /// <returns>WeatherData object or null if failed</returns>
        public async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(city))
            {
                Debug.LogError("City name cannot be empty");
                return null;
            }

            // Check if API key is configured
            if (!ApiConfig.IsApiKeyConfigured())
            {
                Debug.LogError("API key not configured. Please set up your config.json file in StreamingAssets folder.");
                return null;
            }

            //https://api.openweathermap.org/data/2.5/weather?q=london&appid=aada47f1da3cbe0001457c1e95f7f449
            string url = $"{baseUrl}?q={city}&appid={ApiConfig.OpenWeatherMapApiKey}";


            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                // send the web request in the request object we just created
                await request.SendWebRequest();

                while (!request.isDone)
                    await Task.Yield();


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

            }

            return null;// Placeholder - students will replace this
        }



        WeatherData ParseWeatherData(string jsonString)
        {
            return JsonConvert.DeserializeObject<WeatherData>(jsonString);
        }

        /// <summary>
        /// Example usage method - students can use this as reference
        /// </summary>
        private async void Start()
        {
            // Example: Get weather for London
            var weatherData = await GetWeatherDataAsync("London");

            if (weatherData != null && weatherData.IsValid)
            {
                Debug.Log($"Weather in {weatherData.CityName}: {weatherData.TemperatureInCelsius:F1}°C");
                Debug.Log($"Description: {weatherData.PrimaryDescription}");
            }
            else
            {
                Debug.LogError("Failed to get weather data");
            }
        }

    }
} 