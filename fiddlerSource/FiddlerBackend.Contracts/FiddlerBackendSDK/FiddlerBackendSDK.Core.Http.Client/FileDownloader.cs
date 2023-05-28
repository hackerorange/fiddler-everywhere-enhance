using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using Microsoft.Extensions.Logging;

namespace FiddlerBackendSDK.Core.Http.Client;

public class FileDownloader : IFileDownloader
{
	public static readonly int ChunkSize;

	private static readonly SemaphoreSlim DownloadSemaphore;

	private readonly HttpClient plainHttpClient;

	private readonly ILogger<FileDownloader> logger;

	private readonly IBackendConfiguration configuration;

	static FileDownloader()
	{
		ChunkSize = 1048576;
		DownloadSemaphore = new SemaphoreSlim(Environment.ProcessorCount);
	}

	public FileDownloader(IHttpClientFactory httpClientFactory, ILogger<FileDownloader> logger, IBackendConfiguration configuration)
	{
		plainHttpClient = httpClientFactory.CreateClient("External");
		this.logger = logger;
		this.configuration = configuration;
	}

	public async Task DownloadFileAsync(string fileUrl, string targetPath, IFiddlerHttpStatusCodeValidator statusCodeValidator)
	{
		long? num = ((!configuration.ByteRangeDownloadEnabled) ? null : (await GetByteRangeDownloadableLengthAsync(fileUrl)));
		long? num2 = num;
		bool flag = !num2.HasValue;
		if (!flag)
		{
			flag = !(await ByteRangeDownloadAsync(fileUrl, num2.Value, targetPath, statusCodeValidator));
		}
		if (!flag)
		{
			return;
		}
		using HttpResponseMessage httpResponseMessage = await plainHttpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead);
		await statusCodeValidator.Validate(httpResponseMessage);
		using Stream responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
		await using FileStream fs = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.Read);
		await responseStream.CopyToAsync(fs);
	}

	private async Task<bool> ByteRangeDownloadAsync(string fileUrl, long length, string targetPath, IFiddlerHttpStatusCodeValidator statusCodeValidator)
	{
		using (FileStream fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write, FileShare.Write))
		{
			fileStream.SetLength(length);
		}
		Task<bool>[] tasks = (from i in Enumerable.Range(0, (int)Math.Ceiling((double)length / (double)ChunkSize))
			select DownloadTask(fileUrl, targetPath, i * ChunkSize, (i + 1) * ChunkSize, statusCodeValidator)).ToArray();
		await Task.WhenAll(tasks);
		return tasks.All((Task<bool> x) => x.Result);
	}

	private async Task<long?> GetByteRangeDownloadableLengthAsync(string fileUrl)
	{
		HttpRequestMessage request = new HttpRequestMessage
		{
			Method = HttpMethod.Head,
			RequestUri = new Uri(fileUrl)
		};
		using HttpResponseMessage httpResponseMessage = await plainHttpClient.SendAsync(request);
		if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
		{
			HttpContent content = httpResponseMessage.Content;
			if (content != null && content.Headers?.ContentLength.GetValueOrDefault() > 0 && httpResponseMessage.Headers?.AcceptRanges != null && httpResponseMessage.Headers.AcceptRanges.Contains("bytes"))
			{
				return httpResponseMessage.Content.Headers.ContentLength;
			}
		}
		return null;
	}

	private async Task<bool> DownloadTask(string fileUrl, string targetPath, long from, long to, IFiddlerHttpStatusCodeValidator statusCodeValidator)
	{
		await DownloadSemaphore.WaitAsync();
		try
		{
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(fileUrl)
			};
			httpRequestMessage.Headers.Range = new RangeHeaderValue(from, to);
			using HttpResponseMessage response = await plainHttpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
			await statusCodeValidator.Validate(response);
			using (Stream stream = new FileStream(targetPath, FileMode.Open, FileAccess.Write, FileShare.Write))
			{
				stream.Seek(from, SeekOrigin.Begin);
				using Stream responseStream = await response.Content.ReadAsStreamAsync();
				await responseStream.CopyToAsync(stream);
			}
			return true;
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError((ILogger)(object)logger, ex, $"Error downloading part from: ${from} to ${to} of file from url: {fileUrl}", Array.Empty<object>());
			return false;
		}
		finally
		{
			DownloadSemaphore.Release();
		}
	}
}
