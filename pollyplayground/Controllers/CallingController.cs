using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Retry;

namespace pollyplayground.Controllers;

[ApiController]
[Route("[controller]")]
public class CallingController : ControllerBase
{
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
    private readonly IHttpClientFactory httpClientFactory;

    public CallingController(IHttpClientFactory httpClientFactory)
    {
        _retryPolicy = Policy.HandleResult<HttpResponseMessage>(msg => !msg.IsSuccessStatusCode).RetryAsync(3);
        this.httpClientFactory = httpClientFactory;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("https://localhost:50001");
        var endpoint = $"called/{id}";

        var response = await _retryPolicy.ExecuteAsync(() => httpClient.GetAsync(endpoint));

        if (response.IsSuccessStatusCode)
            return Ok("All good!");

        return BadRequest("Invalid Id");
    }
}
