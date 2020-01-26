using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XTMF2.Web.Client.Services;

namespace XTMF2.Web.Client.Util {
    /// <summary>
    /// Base API client with share logic for manipulating request header variables before being sent to the server.
    /// </summary>
    public class BaseClient {

        protected AuthorizationService AuthorizationService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<HttpRequestMessage> CreateHttpRequestMessageAsync (CancellationToken token) {

            var req = new System.Net.Http.HttpRequestMessage ();
            req.Headers.Add ("TestHeader", "Hello");
            return Task.FromResult (req);
        }

    }
}