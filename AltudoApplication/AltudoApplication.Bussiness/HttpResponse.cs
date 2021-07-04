using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AltudoApplication.Business
{
    public class HttpResponse
    {
        public HttpResponseMessage Response { get; set; }
        public string Content { get; set; }
    }
}
