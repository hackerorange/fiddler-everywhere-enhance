using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.AutoResponder.Client;

public class RuleSetsClient : BaseEntityClient, IRuleSetsClient
{
	private const string RuleSetsRelativePath = "rulesets";

	private readonly IFiddlerHttpClient fiddlerHttpClient;

	private readonly IBackendConfiguration backendConfiguration;

	private readonly IFiddlerHttpStatusCodeValidator getRulesStatusCodeValidator;

	public RuleSetsClient(IFiddlerHttpClient fiddlerHttpClient, IFileClient fileClient, IFileDownloader fileDownloader, IMD5Calculator md5Calculator, IValidationExceptionFactory exceptionFactory, IBackendConfiguration backendConfiguration)
		: base(fileClient, fileDownloader, md5Calculator, exceptionFactory)
	{
		this.fiddlerHttpClient = fiddlerHttpClient;
		this.backendConfiguration = backendConfiguration;
		getRulesStatusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(base.ExceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
	}

	public async Task<RuleSetDTO> GetAsync(Guid ruleSetId)
	{
		string requestUri = string.Format("{0}/{1}", "rulesets", ruleSetId);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(base.ExceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		return await fiddlerHttpClient.GetAsync<RuleSetDTO>(requestUri, statusCodeValidator);
	}

	public async Task<ICollection<RuleSetDTO>> GetMineAsync()
	{
		string requestUri = "rulesets/mine";
		return await fiddlerHttpClient.GetAsync<ICollection<RuleSetDTO>>(requestUri, getRulesStatusCodeValidator);
	}

	public async Task<ICollection<RuleSetDTO>> GetAllSharedWithMeAsync()
	{
		string requestUri = "rulesets/shared-with-me";
		return await fiddlerHttpClient.GetAsync<ICollection<RuleSetDTO>>(requestUri, getRulesStatusCodeValidator);
	}

	public async Task<ICollection<RuleSetDTO>> GetAvailableAsync()
	{
		string requestUri = "rulesets";
		return await fiddlerHttpClient.GetAsync<ICollection<RuleSetDTO>>(requestUri, getRulesStatusCodeValidator);
	}

	public async Task<ICollection<AutoResponderRuleBlobs>> DownloadRulesAsync(Guid id, string outputPath)
	{
		RuleSetDTO ruleSetDTO = await GetAsync(id);
		outputPath = Path.Join((ReadOnlySpan<char>)outputPath, (ReadOnlySpan<char>)$"{id}");
		if (!Directory.Exists(outputPath))
		{
			Directory.CreateDirectory(outputPath);
		}
		List<AutoResponderRuleBlobs> ruleSetBlobs = new List<AutoResponderRuleBlobs>();
		foreach (AutoResponderRuleDTO rule in ruleSetDTO.Rules)
		{
			AutoResponderRuleBlobs blobs = new AutoResponderRuleBlobs
			{
				Id = rule.Id
			};
			if (rule.Headers != null)
			{
				blobs.HeadersFile = Path.Combine(outputPath, $"{rule.Id}_headers");
				await DownloadFileAsync(rule.Headers, blobs.HeadersFile);
			}
			if (rule.Body != null)
			{
				blobs.BodyFile = Path.Combine(outputPath, $"{rule.Id}_body");
				await DownloadFileAsync(rule.Body, blobs.BodyFile);
			}
			ruleSetBlobs.Add(blobs);
		}
		return ruleSetBlobs;
	}

	public async Task<RuleSetDTO> CreateAsync(Guid accountId, RuleSet ruleSet)
	{
		string resourcePath = "rulesets";
		IFiddlerHttpStatusCodeValidator validator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(base.ExceptionFactory).WithSuccessCode(HttpStatusCode.Created).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PaymentRequired, HttpStatusCode.BadRequest, HttpStatusCode.Conflict, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		CreateRuleSetDTO createRuleSetDTO = new CreateRuleSetDTO
		{
			AccountId = accountId,
			Name = ruleSet.Name
		};
		foreach (AutoResponderRule rule in ruleSet.Rules)
		{
			CreateAutoResponderRuleDTO createAutoResponderRuleDTO = new CreateAutoResponderRuleDTO
			{
				Match = rule.Match,
				Action = rule.Action,
				Comment = rule.Comment
			};
			if (rule.Headers != null)
			{
				createAutoResponderRuleDTO.HeadersFileId = await UploadFileToS3Async(accountId, rule.Headers);
			}
			if (rule.Body != null)
			{
				createAutoResponderRuleDTO.BodyFileId = await UploadFileToS3Async(accountId, rule.Body);
			}
			createRuleSetDTO.Rules.Add(createAutoResponderRuleDTO);
		}
		return await fiddlerHttpClient.PutAsync<CreateRuleSetDTO, RuleSetDTO>(resourcePath, createRuleSetDTO, validator);
	}

	public async Task<RuleSetDTO> UpdateSharesAsync(Guid ruleSetId, IEnumerable<string> newEmails, string reason, string concurrencyToken)
	{
		string requestUri = string.Format("{0}/{1}/shares", "rulesets", ruleSetId);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(base.ExceptionFactory).WithSuccessCodes(HttpStatusCode.Created, HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized, HttpStatusCode.PaymentRequired, HttpStatusCode.PreconditionFailed, HttpStatusCode.Forbidden).Create();
		List<RuleSetShareDTO> resource = newEmails.Select((string email) => new RuleSetShareDTO
		{
			Email = email,
			Note = reason,
			Permissions = Roles.All
		}).ToList();
		return await fiddlerHttpClient.PostAsync<IEnumerable<RuleSetShareDTO>, RuleSetDTO>(requestUri, resource, statusCodeValidator, CreateModificationRequestHeaders(concurrencyToken));
	}

	public async Task DeleteAsync(Guid ruleSetId, string concurrencyToken)
	{
		string requestUri = string.Format("{0}/{1}", "rulesets", ruleSetId);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(base.ExceptionFactory).WithSuccessCodes(HttpStatusCode.NoContent, HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		await fiddlerHttpClient.DeleteAsync(requestUri, statusCodeValidator, CreateModificationRequestHeaders(concurrencyToken));
	}

	private IEnumerable<(string, string)> CreateModificationRequestHeaders(string concurrencyToken)
	{
		if (!backendConfiguration.DisableConcurrency)
		{
			return new List<(string, string)> { ("If-Match", concurrencyToken) };
		}
		return new List<(string, string)>();
	}

	private async Task<Guid> UploadFileToS3Async(Guid accountId, Stream stream)
	{
		stream.Seek(0L, SeekOrigin.Begin);
		List<byte[]> source = GetFileChunks(stream).ToList();
		string chunkChecksums = string.Join(';', source.Select((byte[] chunk) => base.MD5Calculator.Calculate(chunk)));
		stream.Seek(0L, SeekOrigin.Begin);
		Dictionary<string, string> uploadHeaders = GetUploadHeaders(stream, chunkChecksums);
		InitiateMultipartUploadResponseDTO multipartUploadResponse = await base.FileClient.InitiateMultipartFileUploadAsync(accountId, uploadHeaders);
		IEnumerable<PartETagDTO> etags = await base.FileClient.UploadToS3Async(stream, multipartUploadResponse.UploadUrls.ToList());
		Guid fileId = multipartUploadResponse.FileId;
		await base.FileClient.CompleteFileUploadAsync(accountId, fileId, etags);
		return fileId;
	}

	private IList<byte[]> GetFileChunks(Stream stream)
	{
		byte[] array = new byte[base.FileClient.ChunkSize];
		List<byte[]> list = new List<byte[]>();
		int num;
		while ((num = stream.Read(array, 0, array.Length)) > 0)
		{
			byte[] array2 = new byte[num];
			Buffer.BlockCopy(array, 0, array2, 0, num);
			list.Add(array2);
		}
		return list;
	}

	private Dictionary<string, string> GetUploadHeaders(Stream stream, string chunkChecksums)
	{
		long length = stream.Length;
		string value = base.MD5Calculator.Calculate(stream);
		return new Dictionary<string, string>
		{
			{ "X-Upload-Content-Type", "application/octet-stream" },
			{
				"X-Upload-Content-Length",
				length.ToString()
			},
			{ "X-Upload-Content-MD5", value },
			{
				"X-Upload-Chunk-Size",
				base.FileClient.ChunkSize.ToString()
			},
			{ "X-Upload-Chunk-Checksums", chunkChecksums }
		};
	}
}
