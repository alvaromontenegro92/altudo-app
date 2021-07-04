using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AltudoApplication.Business
{
    public class HtmlManager
    {
        public HttpResponse GetSourceCodeFromWebSite(Uri url)
        {
            var result = new HttpResponse();

            if (!ValidadeURL(url))
                return new HttpResponse() { 
                    Response = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound)
                    , Content = String.Empty
                };
            
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url.AbsoluteUri).Result)
                {
                    result.Response = response;

                    using (HttpContent content = response.Content)
                    {
                        result.Content = content.ReadAsStringAsync().Result;
                    }
                }
            }

            return result;
        }
        private bool ValidadeURL(Uri url)
        {
            return Uri.TryCreate(url.AbsoluteUri, UriKind.Absolute, out url)
                            && (url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps);
        }
    }
}
