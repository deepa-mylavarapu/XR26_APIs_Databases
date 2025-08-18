using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WeatherApp.Services;
using WeatherApp.Data;

namespace WeatherApp.UI
{
    /// <summary>
    /// UI Controller for the Weather Application
    /// Students will connect this to the API client and handle user interactions
    /// </summary>
    public class WeatherUIController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField cityInputField;
        [SerializeField] private Button getWeatherButton;
        [SerializeField] private TextMeshProUGUI weatherDisplayText;
        [SerializeField] private TextMeshProUGUI statusText;

        [Header("API Client")]
        [SerializeField] private WeatherApiClient apiClient;

        private void Start()
        {
            // Set up button click listener
            getWeatherButton.onClick.AddListener(OnGetWeatherClicked);

            // Initialize UI state
            SetStatusText("Enter a city name and click Get Weather");
        }

        /// TODO: Students will implement this method
        private async void OnGetWeatherClicked()
        {
            // Get city name from input field
            string cityName = cityInputField.text;

            // Validate input
            if (string.IsNullOrWhiteSpace(cityName))
            {
                SetStatusText("Please enter a city name");
                return;
            }

            // Disable button and show loading state
            getWeatherButton.interactable = false;
            SetStatusText("Loading weather data...");
            weatherDisplayText.text = "";

            try
            {
                var weatherData = await apiClient.GetWeatherDataAsync(cityName);

                if (weatherData != null && weatherData.IsValid)
                {
                    DisplayWeatherData(weatherData);
                    SetStatusText("Weather data loaded successfully");
                }
                else
                {
                    SetStatusText("Failed to get weather data. Please try again.");
                }
            }
            catch (System.Exception ex)
            {
                // Handle exceptions
                Debug.LogError($"Error getting weather data: {ex.Message}");
                SetStatusText("An error occurred. Please try again.");
            }
            finally
            {
                // Re-enable button
                getWeatherButton.interactable = true;
            }
        }

        private void DisplayWeatherData(WeatherData weatherData)
        {
            string displayText = "";

            if (weatherData.Main != null)
            {
                displayText += $"City: {weatherData.CityName}\n";
                displayText += $"Temperature: {weatherData.TemperatureInCelsius:F1}°C (Feels like: {weatherData.FeelsLikeInCelsius:F1}°C)\n";
                displayText += $"Humidity: {weatherData.Humidity}%\n";
                displayText += $"Pressure: {weatherData.Pressure} hPa\n";
            }

            if (weatherData.Weather != null && weatherData.Weather.Length > 0)
            {
                displayText += $"Description: {weatherData.PrimaryDescription}";
            }

            weatherDisplayText.text = displayText;
        }

        private void SetStatusText(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
        }

        public void ClearDisplay()
        {
            weatherDisplayText.text = "";
            cityInputField.text = "";
            SetStatusText("Enter a city name and click Get Weather");
        }
    }
}