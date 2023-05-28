using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Core.Http.Client;

internal static class HttpClientExtensions
{
	internal static async Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, string requestUri, HttpContent httpContent, IEnumerable<(string, string)> headers = null)
	{
		return await SendAsync(httpClient, requestUri, HttpMethod.Post, httpContent, headers);
	}

	internal static async Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, string requestUri, HttpContent httpContent, IEnumerable<(string, string)> headers = null)
	{
		return await SendAsync(httpClient, requestUri, HttpMethod.Put, httpContent, headers);
	}

	internal static async Task<HttpResponseMessage> PatchAsync(this HttpClient httpClient, string requestUri, HttpContent httpContent, IEnumerable<(string, string)> headers = null)
	{
		return await SendAsync(httpClient, requestUri, HttpMethod.Patch, httpContent, headers);
	}

	internal static async Task<HttpResponseMessage> DeleteAsync(this HttpClient httpClient, string requestUri, IEnumerable<(string, string)> headers = null)
	{
		return await SendAsync(httpClient, requestUri, HttpMethod.Delete, null, headers);
	}

	private static async Task<HttpResponseMessage> SendAsync(HttpClient httpClient, string requestUri, HttpMethod httpMethod, HttpContent httpContent, IEnumerable<(string, string)> headers = null)
	{
		using HttpRequestMessage requestMessage = new HttpRequestMessage(httpMethod, requestUri)
		{
			Content = httpContent
		};
		if (headers != null)
		{
			foreach (var (name, text) in headers)
			{
				requestMessage.Headers.Add(name, new string[1] { text });
			}
		}
		return await httpClient.SendAsync(requestMessage);
	}
}
