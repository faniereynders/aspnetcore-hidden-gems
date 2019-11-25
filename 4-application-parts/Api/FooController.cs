using Microsoft.AspNetCore.Mvc;

namespace Api
{

    public class FooController: ControllerBase
    {
        [HttpGet("external/foo")]
        public IActionResult Get() => Ok("Hello from Foo!");
    }
}
