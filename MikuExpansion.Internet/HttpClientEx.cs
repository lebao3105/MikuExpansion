using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using MikuExpansion.Helpers;

namespace MikuExpansion.Internet
{
    public sealed class UriStringType : MultiTypeInfo
    {
        protected override TypeAndCtorDictionary GetTypes()
            => new TypeAndCtorDictionary
                {
                    [typeof(Uri)] = (s => new Uri((string)s)),
                    [typeof(string)] = (u => (u as Uri).OriginalString)
                };
    }

    public class HttpClientEx : HttpClient
    {
        private MultiType<UriStringType> BaseUri;
        public new Uri BaseAddress
        {
            get { return BaseUri.Get<Uri>(); }
            set { BaseUri.Set(value); }
        }

        public HttpClientEx() : base() { }
        public HttpClientEx(HttpMessageHandler handler) : base(handler) { }
        public HttpClientEx(HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler) { }

        public HttpClientEx(MultiType<UriStringType> BaseUri) : base()
        {
            this.BaseUri = BaseUri;
        }

        public HttpClientEx(HttpMessageHandler handler, MultiType<UriStringType> BaseUri) : base(handler)
        {
            this.BaseUri = BaseUri;
        }

        public HttpClientEx(HttpMessageHandler handler, bool disposeHandler, MultiType<UriStringType> BaseUri) : base(handler)
        {
            this.BaseUri = BaseUri;
        }

        public async Task<HttpResponseMessage> RequestFromStringAsync(
            HttpMethod method, string content,
            Dictionary<string, string> Headers
        )
        {
            HttpRequestMessage request = new HttpRequestMessage(method, BaseAddress);
            foreach (var pair in Headers)
                request.Headers.Add(pair.Key, pair.Value);
            return await SendAsync(request);
        }
    }

    public static class Extensions
    {
        public static StringContent ToStringContent(
            this string self, Encoding encoding = null,
            string mimetype = "application/json")
            => new StringContent(self, encoding, mimetype);
    }
}
