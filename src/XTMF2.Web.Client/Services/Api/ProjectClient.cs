using System;
using XTMF2.Web.Client.Util;

namespace XTMF2.Web.Client.Services.Api {
    public partial class ProjectClient : BaseClient {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        public ProjectClient (System.Net.Http.HttpClient httpClient, AuthorizationService authorization) : this (httpClient) {
            Console.WriteLine ("In here with information about the authorization state");
            AuthorizationService = authorization;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="request"></param>
        /// <param name="urlBuilder"></param>
        /// <returns></returns>
        partial void PrepareRequest (System.Net.Http.HttpClient client, System.Net.Http.HttpRequestMessage request, System.Text.StringBuilder urlBuilder) {
            request.Headers.Add ("Test", "Test2");
        }

    }
}