using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using FiddlerBackendSDK.Core.Http.Client.Validation;

namespace FiddlerBackendSDK.Core.Http.Client;

public interface IAuthenticatedHttpClient
{
	string VersionHeaderName { get; }

	void ReloadToken();

	Task<string> GetRedirectionAsync(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator);

	Task<IEnumerable<T>> GetAllAsync<T>(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator);

	Task<T> GetAsync<T>(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator);

	Task<T> GetAsync<T>(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null);

	Task PutAsync(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null);

	Task<T> PutAsync<T>(string requestUri, T resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null);

	Task<TRes> PutAsync<TReq, TRes>(string requestUri, TReq resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null);

	Task PostAsync(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null);

	Task PostAsync<T>(string requestUri, T resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null);

	Task<TRes> PostAsync<TReq, TRes>(string requestUri, TReq resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null);

	Task<T> PatchAsync<T>(string requestUri, T resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null);

	Task<TRes> PatchAsync<TReq, TRes>(string requestUri, TReq resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null);

	Task DeleteAsync(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null);

	Task<HttpResponseMessage> SendAsync(Func<HttpRequestMessage> getHttpRequestMessage);

	Task<HttpResponseMessage> GetExternalAsync(string requestAbsoluteUri, IFiddlerHttpStatusCodeValidator statusCodeValidator, HttpCompletionOption completionOption);

	Task<HttpResponseMessage> SendExternalAsync(Func<HttpRequestMessage> getHttpRequestMessage, HttpCompletionOption completionOption);
}
