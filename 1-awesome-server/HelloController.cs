using Microsoft.AspNetCore.Mvc;

namespace AwesomeServer
{
    [Route("api/[controller]")]
    public class HelloController
    {
        public string Get() 
        {
            return "Hello World!";
                }

        [HttpGet("json")]
        public object GetJson() => new { FirstName = "Fanie", LastName = "Reynders" };

        [HttpGet("image")]
        public IActionResult GetImage()
        {
            var image = System.IO.File.OpenRead("obama.png");
            return new FileStreamResult(image, "image/png");
        }
    }
}
