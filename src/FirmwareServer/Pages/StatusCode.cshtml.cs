using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FirmwareServer.Pages
{
    public class StatusCodeModel : PageModel
    {
        private readonly ILogger<StatusCodeModel> _logger;

        public StatusCodeModel(ILogger<StatusCodeModel> logger)
        {
            _logger = logger;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public void OnGet(int code)
        {
            var reExecute = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            _logger.LogWarning($"Unexpected Status Code: {code}, OriginalPath: {reExecute.OriginalPath}");

            Code = code;
            Message = string.Empty;

            switch (Code)
            {
                case 400:
                    Message = "Bad request: The request cannot be fulfilled due to bad syntax";
                    break;

                case 403:
                    Message = "Forbidden";
                    break;

                case 404:
                    Message = "Page not found";
                    break;

                case 408:
                    Message = "The server timed out waiting for the request";
                    break;

                case 500:
                    Message = "Internal Server Error - server was unable to finish processing the request";
                    break;

                default:
                    Message = "That’s odd... Something we didn't expect happened";
                    break;
            }
        }
    }
}