#region usings
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
#endregion

namespace AwesomeServer
{
    public class FileHttpResponse : HttpResponse
    {
        private readonly string path;
        public FileHttpResponse(HttpContext httpContext, string path)
        {
            this.HttpContext = httpContext;
            this.path = path;
        }


        public override void OnCompleted(Func<object, Task> callback, object state)
        {
            var file = GetFileName();
            using (var reader = new StreamReader(Body))
            using (var fs = new FileStream(file, FileMode.CreateNew))
            {
                
                Body.Seek(0, SeekOrigin.Begin);
                Body.CopyTo(fs);
                Body.Flush();
                Body.Dispose();
            }
        }


        #region N/A
        public override HttpContext HttpContext { get; }
        public override int StatusCode { get; set; }
        public override IHeaderDictionary Headers => new HeaderDictionary();
        public override Stream Body { get; set; } = new MemoryStream();
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override IResponseCookies Cookies => throw new NotImplementedException();
        public override bool HasStarted => true;
        public override void OnStarting(Func<object, Task> callback, object state) { }
        public override void Redirect(string location, bool permanent) { }

        private string GetFileName()
        {
            var info = new FileInfo(path);
            var ext = info.Extension;
            if (ContentType.Contains("json"))
            {
                ext = ".json";
            }
            else if (ContentType.Contains("png"))
            {
                ext = ".png";
            }

            return Path.Combine(info.Directory.FullName, DateTime.Now.Ticks.ToString() + "_" + info.Name.Replace(info.Extension, ext));
            
        }
        #endregion
    }


}
