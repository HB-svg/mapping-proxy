using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace MappingProxy.Controllers
{
    [ApiController]
    [Route("proxy")]
    public class ProxyController : ControllerBase
    {
        private readonly HttpClient _client;

        public ProxyController(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return BadRequest("Missing url parameter");

            var resp = await _client.GetAsync(url);
            var bytes = await resp.Content.ReadAsByteArrayAsync();

            // Allow calls from your front-end
            Response.Headers.Add("Access-Control-Allow-Origin", "*");

            return File(bytes, resp.Content.Headers.ContentType?.ToString() ?? "application/octet-stream");
        }
    }
}
