#region usings
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using System.Security.Claims;
using System.Threading;
using System.IO;
#endregion

namespace AwesomeServer
{
    public class AwesomeHttpContext : HttpContext
    {
        public AwesomeHttpContext(IFeatureCollection features, string inPath, string outPath)
        {
            this.Features = features;
            this.Request = new FileHttpRequest(this, inPath);

            var fileName = new FileInfo(inPath).Name;
            var outFileName = Path.Combine(outPath, fileName);

            this.Response = new FileHttpResponse(this, outFileName);
            this.RequestServices = features.Get<IServiceProvidersFeature>().RequestServices;
        }

        public override HttpRequest Request { get; }
        public override HttpResponse Response { get; }
        public override IFeatureCollection Features { get; }
        public override IServiceProvider RequestServices { get; set; }
        

        #region N/A
        public override CancellationToken RequestAborted { get; set; }
        public override ConnectionInfo Connection => throw new NotImplementedException();
        public override WebSocketManager WebSockets => throw new NotImplementedException();
        [Obsolete]public override AuthenticationManager Authentication => throw new NotImplementedException();
        public override ClaimsPrincipal User { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override void Abort() => throw new NotImplementedException(); 
        #endregion
    }


}
