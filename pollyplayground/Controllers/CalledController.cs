
using Microsoft.AspNetCore.Mvc;

namespace pollyplayground.Controllers;

[ApiController]
[Route("[controller]")]
public class CalledController : ControllerBase
{
    private static int _counter = 0;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        await Task.Delay(100);
        _counter++;
        if (_counter % 4 == 0)
            return Ok();

        return BadRequest();
    }
}
