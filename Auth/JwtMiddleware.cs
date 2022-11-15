using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace SampleExtensionReceiver.Auth
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly JwtOptions _configuration;
        private readonly HttpClient _httpClient;

        public JwtMiddleware(
            RequestDelegate next,
            ILogger<JwtMiddleware> logger,
            IOptions<JwtOptions> configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration.Value;

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            };

            _httpClient = new HttpClient(handler);
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Body.ReadAsync(new Memory<byte>());

            var endpointRequiresAuth = context.Request.Path.Equals("/extensions", StringComparison.CurrentCultureIgnoreCase);

            if (token is null && endpointRequiresAuth)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Missing Authorization token");
                return;
            }

            if (token is not null)
            {
                var (_, error) = await ValidateToken(token);
                if (!string.IsNullOrWhiteSpace(error))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync(error);
                    return;
                }
            }

            await _next.Invoke(context);
        }

        private async Task<(bool result, string error)> ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var jwksJson = await GetJwksJson();
                var jwks = new JsonWebKeySet(jwksJson);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = jwks.Keys.First(),
                    ValidateAudience = false,
                    ValidIssuer = _configuration.Issuer
                }, out _);

                return (true, null);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected error validating JWT");
                return (false, e.GetBaseException().Message);
            }
        }

        private async Task<string> GetJwksJson()
        {
            var response = await _httpClient.GetAsync(_configuration.Uri);
            return await response.Content.ReadAsStringAsync();
        }
    }
}