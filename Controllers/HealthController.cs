using Microsoft.AspNetCore.Mvc;

namespace ClassicsLiteratureAsp.Controllers;

[ApiController]
[Route("/")]
public class HealthController : ControllerBase
{

    [HttpGet]
    public IActionResult Health()
    {
        return Ok("Server is running on port 5225");
    }
}
