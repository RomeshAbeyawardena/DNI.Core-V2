using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNI.Core.Contracts.Services
{
    public interface IHttpService : IDisposable
    {
        Task<T> GetAsync<T>(string requestUrl, CancellationToken cancellationToken, Action<IQueryString> configureQueryString);
        Task<T> PostAsync<TRequest, T>(string requestUrl, TRequest requestBody, CancellationToken cancellationToken);
        Task<T> RequestAsync<T>(string requestUrl, 
            HttpMethod httpMethod, 
            HttpCompletionOption httpCompletionOption, 
            CancellationToken cancellationToken,
            Action<HttpRequestMessage> configureRequestMessage = null);
    }
}
