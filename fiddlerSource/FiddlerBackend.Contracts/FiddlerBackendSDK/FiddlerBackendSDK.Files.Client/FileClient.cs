using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using Newtonsoft.Json;

namespace FiddlerBackendSDK.Files.Client;

public class FileClient : IFileClient
{
	private readonly string filesRelativePath = "files";

	private readonly long chunkSize = 5242880L;

	private readonly IFiddlerHttpClient fiddlerHttpClient;

	private readonly IValidationExceptionFactory exceptionFactory;

	private readonly IMD5Calculator mD5Calculator;

	public long ChunkSize => chunkSize;

	public FileClient(IFiddlerHttpClient fiddlerHttpClient, IValidationExceptionFactory exceptionFactory, IMD5Calculator mD5Calculator)
	{
		this.fiddlerHttpClient = fiddlerHttpClient;
		this.exceptionFactory = exceptionFactory;
		this.mD5Calculator = mD5Calculator;
	}

	public async Task<string> GetFileUrlAsync(Guid fileId)
	{
		string requestUri = $"{filesRelativePath}/{fileId}";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.Found).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
		return await fiddlerHttpClient.GetRedirectionAsync(requestUri, statusCodeValidator);
	}

	public async Task<InitiateMultipartUploadResponseDTO> InitiateMultipartFileUploadAsync(Guid accountId, Dictionary<string, string> headers)
	{
		AccountIdDTO accountIdDTO = new AccountIdDTO
		{
			AccountId = accountId
		};
		string serializedResource = JsonConvert.SerializeObject((object)accountIdDTO);
		using HttpResponseMessage response = await fiddlerHttpClient.SendAsync(delegate
		{
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri(filesRelativePath ?? "", UriKind.Relative),
				Content = new StringContent(serializedResource, Encoding.UTF8, "application/json")
			};
			foreach (KeyValuePair<string, string> header in headers)
			{
				httpRequestMessage.Headers.Add(header.Key, header.Value);
			}
			httpRequestMessage.Headers.Add(fiddlerHttpClient.VersionHeaderName, "1.1");
			return httpRequestMessage;
		});
		await new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.BadRequest, HttpStatusCode.PaymentRequired, HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create()
			.Validate(response);
		return JsonConvert.DeserializeObject<InitiateMultipartUploadResponseDTO>(await response.Content.ReadAsStringAsync());
	}

	public async Task<IEnumerable<PartETagDTO>> UploadToS3Async(Stream input, IList<string> uploadUrls)
	{
		if (uploadUrls == null || uploadUrls.Count == 0)
		{
			throw new ArgumentException("Upload URLs collections should not be empty!");
		}
		if (input == null)
		{
			throw new ArgumentException("The input stream cannot be null!");
		}
		if (input.Length < 1)
		{
			throw new ArgumentException("The input stream should not be empty!");
		}
		long num = input.Length % ChunkSize;
		long num2 = input.Length / ChunkSize;
		if (num > 0)
		{
			num2++;
		}
		if (num2 != uploadUrls.Count)
		{
			throw new ArgumentException("The number of chunks and upload Urls should match!");
		}
		input.Seek(0L, SeekOrigin.Begin);
		int part = 1;
		byte[] buffer = new byte[ChunkSize];
		List<Task<PartETagDTO>> uploadTasks = new List<Task<PartETagDTO>>();
		SemaphoreSlim throttler = new SemaphoreSlim(Environment.ProcessorCount * 2);
		while (true)
		{
			int num3;
			int bytesRead = (num3 = input.Read(buffer, 0, buffer.Length));
			if (num3 <= 0)
			{
				break;
			}
			await throttler.WaitAsync();
			byte[] chunk = new byte[bytesRead];
			Buffer.BlockCopy(buffer, 0, chunk, 0, bytesRead);
			string uploadUrl = uploadUrls[part - 1];
			int partNumber = part;
			Task<PartETagDTO> item = Task.Run(async delegate
			{
				try
				{
					string eTag = await UploadPartAsync(uploadUrl, chunk);
					return new PartETagDTO
					{
						ETag = eTag,
						PartNumber = partNumber
					};
				}
				finally
				{
					throttler.Release();
				}
			});
			uploadTasks.Add(item);
			part++;
		}
		return await Task.WhenAll(uploadTasks);
	}

	public async Task CompleteFileUploadAsync(Guid accountId, Guid fileId, IEnumerable<PartETagDTO> etags)
	{
		string requestUri = $"{filesRelativePath}/{fileId}";
		FileUploadDTO resource = new FileUploadDTO
		{
			AccountId = accountId,
			Parts = etags.ToList()
		};
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.NoContent).WithErrorCodes(HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.PaymentRequired).Create();
		await fiddlerHttpClient.PostAsync(requestUri, resource, statusCodeValidator);
	}

	private async Task<string> UploadPartAsync(string uploadUrl, byte[] buffer)
	{
		using HttpResponseMessage uploadResult = await fiddlerHttpClient.SendExternalAsync(delegate
		{
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, new Uri(uploadUrl));
			httpRequestMessage.Content = new ByteArrayContent(buffer);
			httpRequestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
			string value = mD5Calculator.Calculate(buffer);
			httpRequestMessage.Content.Headers.Add("Content-MD5", value);
			return httpRequestMessage;
		}, HttpCompletionOption.ResponseContentRead);
		await new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCodes(HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.NoContent).WithErrorCodes(HttpStatusCode.BadRequest, HttpStatusCode.NotFound).Create()
			.Validate(uploadResult);
		return uploadResult.Headers?.ETag?.ToString()?.Replace("\"", string.Empty) ?? throw new ArgumentException("No ETag received from S3!");
	}
}
