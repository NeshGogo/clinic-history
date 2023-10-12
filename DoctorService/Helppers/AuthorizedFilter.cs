using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Text;
using System.Text.Json;

namespace DoctorService.Helppers
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class AuthorizedFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AuthorizedFilter> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string? _role;

        public AuthorizedFilter(
             string? role,
            IConfiguration config, 
            ILogger<AuthorizedFilter> logger, 
            IHttpClientFactory clientFactory)
        {

            _config = config;
            _logger = logger;
            _clientFactory = clientFactory;
            _role = role;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var response = new ContentResult { ContentType = "application/json" };
            try
            {
                var client = _clientFactory.CreateClient();
                var host = _config["Services-host:AccountService"];
                var uri = new Uri($"{host}/api/auth/Authorized");
                var token = context.HttpContext.Request.Headers.Authorization.ToString();
                
                var responseMessage = await client.PostAsJsonAsync(uri, token);
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
