using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.Mapping;

public class FileDTOToBlobConverter : ITypeConverter<FileDTO, IBlobResource<string>>
{
	private readonly IFileClient fileClient;

	private readonly HttpClient plainHttpClient;

	private readonly IValidationExceptionFactory exceptionFactory;

	public FileDTOToBlobConverter(IHttpClientFactory httpClientFactory, IFileClient fileClient, IValidationExceptionFactory exceptionFactory)
	{
		plainHttpClient = httpClientFactory.CreateClient("External");
		this.fileClient = fileClient;
		this.exceptionFactory = exceptionFactory;
	}

	public IBlobResource<string> Convert(FileDTO source, IBlobResource<string> destination, ResolutionContext context)
	{
		if (source != null)
		{
			_ = source.Id;
			return new RemoteBlobResource<string>(source.Id, DownloadFileAsync);
		}
		return new LocalBlobResource<string>(string.Empty);
	}

	private async Task<string> DownloadFileAsync(Guid? remoteFileId)
	{
		if (!remoteFileId.HasValue)
		{
			return string.Empty;
		}
		string requestUri = await fileClient.GetFileUrlAsync(remoteFileId.Value);
		IFiddlerHttpStatusCodeValidator validator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCodes(HttpStatusCode.OK, HttpStatusCode.PartialContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
		using HttpResponseMessage httpResponseMessage = await plainHttpClient.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
		await validator.Validate(httpResponseMessage);
		return await httpResponseMessage.Content.ReadAsStringAsync();
	}
}
