using Microsoft.AspNetCore.Mvc;

namespace AwesomeApi
{
    [Route("api/[controller]")]
    public class ExampleController : Controller
    {
        public ActionResult Post(EmotionalPhotoDto item)
        {
            
            return Ok(item.Scores);
        }
    }
}
