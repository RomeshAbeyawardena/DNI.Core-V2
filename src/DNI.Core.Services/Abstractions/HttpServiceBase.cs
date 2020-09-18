using DNI.Core.Contracts;
using DNI.Core.Contracts.Providers;
using DNI.Core.Contracts.Services;
using DNI.Core.Domains;
using DNI.Core.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Services.Abstractions
{
    public abstract class HttpServiceBase : IHttpService
    {
        protected HttpServiceBase(IJsonStreamSerializerProvider jsonStreamSerializerProvider, HttpClient httpClient)
        {
            this.jsonStreamSerializerProvider = jsonStreamSerializerProvider;
            this.httpClient = httpClient;
        }

        protected HttpServiceBase(IJsonStreamSerializerProvider jsonStreamSerializerProvider, HttpMessageHandler httpMessageHandler, bool dispose)
            : this(jsonStreamSerializerProvider, new HttpClient(httpMessageHandler, dispose))
        {

        }

        protected HttpServiceBase(IJsonStreamSerializerProvider jsonStreamSerializerProvider)
            : this(jsonStreamSerializerProvider, new HttpClient())
        {

        }

        protected HttpServiceBase(IJsonStreamSerializerProvider jsonStreamSerializerProvider, Uri uri)
            : this(jsonStreamSerializerProvider)
        {
            httpClient.BaseAddress = uri;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<T> GetAsync<T>(string requestUrl, 
            CancellationToken cancellationToken,
            Action<IQueryString> configureQueryString)
        {
            var queryString = QueryString.Create();
            configureQueryString.Invoke(queryString);

            if(queryString.Count > 0)
                requestUrl = string.Format("{0}?{1}", requestUrl, queryString);

            return RequestAsync<T>(requestUrl, HttpMethod.Get, HttpCompletionOption.ResponseContentRead, cancellationToken);
        }

        public Task<T> PostAsync<TRequest, T>(string requestUrl, TRequest requestBody, CancellationToken cancellationToken)
        {
            var formContent = new FormUrlEncodedContent(requestBody.ToDictionary());

            return RequestAsync<T>(requestUrl, HttpMethod.Post, 
                HttpCompletionOption.ResponseContentRead, cancellationToken,
                configure => configure.Content = formContent);

        }

        public async Task<T> RequestAsync<T>(
            string requestUrl, 
            HttpMethod httpMethod, 
            HttpCompletionOption httpCompletionOption, 
            CancellationToken cancellationToken,
            Action<HttpRequestMessage> configureRequestMessage = null)
        {
            var httpRequestMessage = new HttpRequestMessage(httpMethod, requestUrl);
            configureRequestMessage?.Invoke(httpRequestMessage);
            var response = await httpClient.SendAsync(httpRequestMessage, httpCompletionOption, cancellationToken);

            response.EnsureSuccessStatusCode();
            return await jsonStreamSerializerProvider
                .DeserializeStreamAsync<T>(await response.Content.ReadAsStreamAsync(), cancellationToken);
        }

        protected virtual void Dispose(bool disposing)
        {
            httpClient?.Dispose();
        }

        private readonly IJsonStreamSerializerProvider jsonStreamSerializerProvider;
        private readonly HttpClient httpClient;
    }
}
