using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace DoctorService.Helppers
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AuthorizedFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthorizedFilter> _logger;
        private readonly HttpClient _client;
        private readonly string? _role;

        public AuthorizedFilter(
            IConfiguration config, 
            ILogger<AuthorizedFilter> logger, 
            HttpClient client,
            string? role = null)
        {

            _config = config;
            _logger = logger;
            _client = client;
            _role = role;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var response = new ContentResult { ContentType = "application/json" };
            try
            {
                var host = _config["Services-host:AccountService"];
                var uri = new Uri($"{host}/api/auth/Authorized");
                var token = context.HttpContext.Request.Headers.Authorization.ToString();
                var bodyToSend = new StringContent(JsonSerializer.Serialize(token), Encoding.UTF8, Application.Json);
                var responseMessage = await _client.PostAsync(uri, bodyToSend);
                var content = await responseMessage.Content.ReadAsStringAsync();

                if (!responseMessage.IsSuccessStatusCode)
                {
                    var body = JsonSerializer.Deserialize<dynamic>(content);
                    _logger.LogWarning($"Authorization failed. Authorization state is {body}.");
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Content = "Authorization failed";
                    context.Result = response;
                    return;
                }

                if (!(Convert.ToBoolean(content)))
                {
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.Content = "Authorization failed";
                    context.Result = response;
                    return;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"AuthorizeToken message {ex.Message} StackTrace {ex.StackTrace}.");
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Content = "Internal server error";
                context.Result = response;                
            }
        }
    }
}
