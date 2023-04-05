using Microsoft.AspNetCore.Mvc;

[Route("api/hello")]
public class HelloController
{
    public string Get() =>  "Hello World!";

    [HttpGet("json")]
    public object GetJson() => new { FirstName = "Fanie", LastName = "Reynders" };

    [HttpGet("image")]
    public IActionResult GetImage()
    {
        var image = System.IO.File.OpenRead("obama.png");
        return new FileStreamResult(image, "image/png");
    }
}
