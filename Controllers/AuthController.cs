using System;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Google.Apis.Auth.OAuth2;


namespace api.Controllers
{
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AuthController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("google/login")]
        public IActionResult GoogleLogin()
        {
            // Створення URL для авторизації Google
            var clientId = _configuration["Google:ClientId"];
            var redirectUri = Url.Action("GoogleCallback", "Auth", null, Request.Scheme);
            var scope = "https://www.googleapis.com/auth/calendar.events";

            var authUrl = $"https://accounts.google.com/o/oauth2/v2/auth?response_type=code&client_id={clientId}&redirect_uri={Uri.EscapeDataString(redirectUri)}&scope={Uri.EscapeDataString(scope)}";

            // Перенаправляємо користувача на сторінку авторизації Google
            return Redirect(authUrl);
        }
        [EnableCors("AllowAll")]
        [HttpGet("google/callback")]
        public async Task<IActionResult> GoogleCallback(string code, string state)
        {
            // Перевірка стану (state) для захисту від CSRF
            var expectedState = HttpContext.Session.GetString("state");
            if (expectedState == null || expectedState != state)
            {
                return BadRequest("Invalid state parameter.");
            }

            // Обмін кодом авторизації на токен доступу
            var clientId = _configuration["Google:ClientId"];
            var clientSecret = _configuration["Google:ClientSecret"];
            var redirectUri = Url.Action("GoogleCallback", "Auth", null, Request.Scheme);

            var tokenRequestData = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
            };

            // Виконання POST-запиту для отримання токена доступу
            var tokenResponse = await _httpClient.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(tokenRequestData));
            tokenResponse.EnsureSuccessStatusCode();

            var tokenResponseString = await tokenResponse.Content.ReadAsStringAsync();
            var tokenResponseJson = JsonDocument.Parse(tokenResponseString).RootElement;

            var accessToken = tokenResponseJson.GetProperty("access_token").GetString();
            var refreshToken = tokenResponseJson.GetProperty("refresh_token").GetString();

            // Збереження токена доступу та оновлення для користувача
            HttpContext.Session.SetString("access_token", accessToken);
            HttpContext.Session.SetString("refresh_token", refreshToken);

            // Перенаправлення на головну сторінку або інше місце після успішної авторизації
            return Redirect("/");
        }
    }
}
