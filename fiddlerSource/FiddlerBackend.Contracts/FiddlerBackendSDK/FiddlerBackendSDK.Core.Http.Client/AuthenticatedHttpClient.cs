using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using Newtonsoft.Json;

namespace FiddlerBackendSDK.Core.Http.Client;

public abstract class AuthenticatedHttpClient : IAuthenticatedHttpClient
{
	private readonly IBackendConfiguration configuration;

	private readonly HttpClient httpClient;

	private readonly HttpClient plainHttpClient;

	public string VersionHeaderName => configuration.VersionHeaderName;

	public AuthenticatedHttpClient(IHttpClientFactory httpClientFactory, IBackendConfiguration configuration, string clientConfigurationName)
	{
		this.configuration = configuration;
		httpClient = httpClientFactory.CreateClient(clientConfigurationName);
		plainHttpClient = httpClientFactory.CreateClient("External");
	}

	public async Task DeleteAsync(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator)
	{
		await DeleteAsync(requestUri, statusCodeValidator, new List<(string, string)>());
	}

	public async Task DeleteAsync(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers)
	{
		using HttpResponseMessage response = await ExecuteWithValidTokenAsync(async () => await httpClient.DeleteAsync(requestUri, headers));
		await statusCodeValidator.Validate(response);
	}

	public async Task<IEnumerable<T>> GetAllAsync<T>(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator)
	{
		using HttpResponseMessage httpResponseMessage = await ExecuteWithValidTokenAsync(async () => await httpClient.GetAsync(requestUri));
		await statusCodeValidator.Validate(httpResponseMessage);
		return JsonConvert.DeserializeObject<IEnumerable<T>>(await httpResponseMessage.Content.ReadAsStringAsync());
	}

	public async Task<T> GetAsync<T>(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator)
	{
		return await GetAsync<T>(requestUri, statusCodeValidator, null);
	}

	public async Task<T> GetAsync<T>(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers)
	{
		using HttpResponseMessage httpResponseMessage = await ExecuteWithValidTokenAsync(async delegate
		{
			using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
			if (headers != null)
			{
				foreach (var (name, text) in headers)
				{
					requestMessage.Headers.Add(name, new string[1] { text });
				}
			}
			return await httpClient.SendAsync(requestMessage);
		});
		await statusCodeValidator.Validate(httpResponseMessage);
		return JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
	}

	public async Task<string> GetRedirectionAsync(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator)
	{
		using HttpResponseMessage httpResponseMessage = await ExecuteWithValidTokenAsync(async () => await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead));
		await statusCodeValidator.Validate(httpResponseMessage);
		return httpResponseMessage.Headers.Location.ToString();
	}

	public async Task PostAsync(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null)
	{
		using HttpResponseMessage httpResponseMessage = await ExecuteWithValidTokenAsync(async () => await httpClient.PostAsync(requestUri, null, headers));
		await statusCodeValidator.Validate(httpResponseMessage);
	}

	public async Task PostAsync<T>(string requestUri, T resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null)
	{
		await PostAsync<T, T>(requestUri, resource, statusCodeValidator, headers);
	}

	public async Task<TRes> PostAsync<TReq, TRes>(string requestUri, TReq resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null)
	{
		string serializedResource = JsonConvert.SerializeObject((object)resource);
		using HttpResponseMessage httpResponseMessage = await ExecuteWithValidTokenAsync(async () => await httpClient.PostAsync(requestUri, new StringContent(serializedResource, Encoding.UTF8, "application/json"), headers));
		await statusCodeValidator.Validate(httpResponseMessage);
		return JsonConvert.DeserializeObject<TRes>(await httpResponseMessage.Content.ReadAsStringAsync());
	}

	public async Task<T> PatchAsync<T>(string requestUri, T resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null)
	{
		return await PatchAsync<T, T>(requestUri, resource, statusCodeValidator, headers);
	}

	public async Task<TRes> PatchAsync<TReq, TRes>(string requestUri, TReq resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null)
	{
		string serializedResource = JsonConvert.SerializeObject((object)resource);
		using HttpResponseMessage httpResponseMessage = await ExecuteWithValidTokenAsync(async () => await httpClient.PatchAsync(requestUri, new StringContent(serializedResource, Encoding.UTF8, "application/json"), headers));
		await statusCodeValidator.Validate(httpResponseMessage);
		return JsonConvert.DeserializeObject<TRes>(await httpResponseMessage.Content.ReadAsStringAsync());
	}

	public async Task PutAsync(string requestUri, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null)
	{
		using HttpResponseMessage httpResponseMessage = await ExecuteWithValidTokenAsync(async () => await httpClient.PutAsync(requestUri, null, headers));
		await statusCodeValidator.Validate(httpResponseMessage);
	}

	public async Task<T> PutAsync<T>(string requestUri, T resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null)
	{
		return await PutAsync<T, T>(requestUri, resource, statusCodeValidator, headers);
	}

	public async Task<TRes> PutAsync<TReq, TRes>(string requestUri, TReq resource, IFiddlerHttpStatusCodeValidator statusCodeValidator, IEnumerable<(string, string)> headers = null)
	{
		string serializedResource = JsonConvert.SerializeObject((object)resource);
		using HttpResponseMessage httpResponseMessage = await ExecuteWithValidTokenAsync(async () => await httpClient.PutAsync(requestUri, new StringContent(serializedResource, Encoding.UTF8, "application/json"), headers));
		await statusCodeValidator.Validate(httpResponseMessage);
		return JsonConvert.DeserializeObject<TRes>(await httpResponseMessage.Content.ReadAsStringAsync());
	}

	public Task<HttpResponseMessage> SendAsync(Func<HttpRequestMessage> getHttpRequestMessage)
	{
		return ExecuteWithValidTokenAsync(async () => await httpClient.SendAsync(getHttpRequestMessage()));
	}

	public async Task<HttpResponseMessage> GetExternalAsync(string requestAbsoluteUri, IFiddlerHttpStatusCodeValidator statusCodeValidator, HttpCompletionOption completionOption)
	{
		HttpResponseMessage httpResponseMessage = await plainHttpClient.GetAsync(requestAbsoluteUri, completionOption);
		await statusCodeValidator.Validate(httpResponseMessage);
		return httpResponseMessage;
	}

	public Task<HttpResponseMessage> SendExternalAsync(Func<HttpRequestMessage> getHttpRequestMessage, HttpCompletionOption completionOption)
	{
		return plainHttpClient.SendAsync(getHttpRequestMessage(), completionOption);
	}

	public void ReloadToken()
	{
		httpClient.DefaultRequestHeaders.Remove("Authorization");
		httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + configuration.BearerToken);
	}

	private async Task<HttpResponseMessage> ExecuteWithValidTokenAsync(Func<Task<HttpResponseMessage>> action)
	{
		if (configuration.ReissueToken == null)
		{
			return await action();
		}
		HttpResponseMessage httpResponseMessage = await action();
		if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
		{
			IBackendConfiguration backendConfiguration = configuration;
			backendConfiguration.BearerToken = await configuration.ReissueToken(configuration.BearerToken);
			ReloadToken();
			return await action();
		}
		return httpResponseMessage;
	}
}
