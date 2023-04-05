using Microsoft.AspNetCore.Mvc;

namespace AwesomeApi
{
    [Route("api/[controller]/mvc")]
    public class ExampleController : Controller
    {
        public ActionResult Post(FaceDetectionDto item)
        {
            
            return Ok(item.Faces);
        }
    }
}
