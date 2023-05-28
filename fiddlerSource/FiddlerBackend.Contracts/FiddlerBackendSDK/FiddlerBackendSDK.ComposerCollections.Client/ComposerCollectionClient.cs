using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.ComposerCollections.Client;

public class ComposerCollectionClient : IComposerCollectionClient
{
	private const string ComposerCollectionsApiRelativePath = "composer-collections";

	private readonly IFiddlerHttpClient fiddlerHttpClient;

	private readonly IFileClient fileClient;

	private readonly IMD5Calculator md5Calculator;

	private readonly IValidationExceptionFactory exceptionFactory;

	private readonly IFiddlerHttpStatusCodeValidator getComposerCollectionsStatusCodeValidator;

	private readonly IMapper mapper;

	private readonly IEntityCache<ComposerCollectionCacheItem> collectionsCache;

	private readonly IBackendConfiguration backendConfiguration;

	public ComposerCollectionClient(IFiddlerHttpClient fiddlerHttpClient, IFileClient fileClient, IMD5Calculator md5Calculator, IValidationExceptionFactory exceptionFactory, IMapper mapper, IEntityCache<ComposerCollectionCacheItem> collectionsCache, IBackendConfiguration backendConfiguration)
	{
		this.collectionsCache = collectionsCache;
		this.backendConfiguration = backendConfiguration;
		this.fiddlerHttpClient = fiddlerHttpClient;
		this.fileClient = fileClient;
		this.md5Calculator = md5Calculator;
		this.exceptionFactory = exceptionFactory;
		this.mapper = mapper;
		getComposerCollectionsStatusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(this.exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
	}

	public Task<IList<ComposerCollection>> GetAvailableComposerCollectionsAsync()
	{
		return Task.FromResult((IList<ComposerCollection>)new List<ComposerCollection>());
	}

	public async Task<ComposerCollection> GetComposerCollectionAsync(Guid composerCollectionId)
	{
		ComposerCollectionDTO composerCollectionDTO = await GetComposerCollectionDTOAsync(composerCollectionId);
		collectionsCache.Add(((IMapperBase)mapper).Map<ComposerCollectionCacheItem>((object)composerCollectionDTO));
		return FromDTO(composerCollectionDTO);
	}

	public async Task<ComposerCollection> CreateComposerCollectionAsync(Guid accountId, ComposerCollection composerCollection)
	{
		string resourcePath = "composer-collections";
		IFiddlerHttpStatusCodeValidator validator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.Created).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PaymentRequired, HttpStatusCode.BadRequest, HttpStatusCode.Conflict, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		CreateComposerCollectionDTO resource = await ToDTOAsync(accountId, composerCollection);
		ComposerCollectionDTO composerCollectionDTO = await fiddlerHttpClient.PutAsync<CreateComposerCollectionDTO, ComposerCollectionDTO>(resourcePath, resource, validator);
		collectionsCache.Add(((IMapperBase)mapper).Map<ComposerCollectionCacheItem>((object)composerCollectionDTO));
		return FromDTO(composerCollectionDTO);
	}

	public async Task<ComposerCollection> UpdateComposerCollectionEmailsAsync(Guid composerCollectionId, IEnumerable<string> newEmails, string reason, string concurrencyToken)
	{
		string requestUri = string.Format("{0}/{1}/shares", "composer-collections", composerCollectionId);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PaymentRequired, HttpStatusCode.BadRequest, HttpStatusCode.Conflict, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		List<ShareDTO> source = newEmails.Select((string e) => new ShareDTO
		{
			Email = e,
			Note = reason
		}).ToList();
		return FromDTO(await fiddlerHttpClient.PostAsync<List<ShareDTO>, ComposerCollectionDTO>(requestUri, source.ToList(), statusCodeValidator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken)));
	}

	public async Task<ComposerCollection> UpdateComposerCollectionNameAsync(Guid composerCollectionId, string name, string concurrencyToken)
	{
		return await UpdateComposerCollectionNameAndDescriptionAsync(composerCollectionId, name, null, concurrencyToken);
	}

	public async Task<ComposerCollection> UpdateComposerCollectionNameAndDescriptionAsync(Guid composerCollectionId, string name, string description, string concurrencyToken)
	{
		string requestUri = string.Format("{0}/{1}", "composer-collections", composerCollectionId);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PaymentRequired, HttpStatusCode.BadRequest, HttpStatusCode.Conflict, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		UpdateComposerCollectionBodyDTO resource = new UpdateComposerCollectionBodyDTO
		{
			Name = name,
			Description = description
		};
		return FromDTO(await fiddlerHttpClient.PostAsync<UpdateComposerCollectionBodyDTO, ComposerCollectionDTO>(requestUri, resource, statusCodeValidator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken)));
	}

	public async Task DeleteComposerCollectionAsync(Guid composerCollectionId, string concurrencyToken)
	{
		string requestUri = string.Format("{0}/{1}", "composer-collections", composerCollectionId);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.NoContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		await fiddlerHttpClient.DeleteAsync(requestUri, statusCodeValidator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken));
	}

	public async Task<ComposerCollection> CreateComposerCollectionFoldersAndRequests(Guid composerCollectionId, ComposerCollectionFolder folderToCreate, IEnumerable<ComposerCollectionFolder> childFolders, IEnumerable<ComposerCollectionRequest> childRequests, string concurrencyToken)
	{
		ComposerCollectionCacheItem composerCollectionCacheItem = await collectionsCache.GetAsync(composerCollectionId, GetComposerCollectionCacheItemAsync);
		string resourcePath = string.Format("{0}/{1}", "composer-collections", composerCollectionId);
		IFiddlerHttpStatusCodeValidator validator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.BadRequest, HttpStatusCode.PaymentRequired, HttpStatusCode.Forbidden).Create();
		ComposerCollectionMultipleFoldersAndRequestsCreateBodyDTO composerCollectionMultipleFoldersAndRequestsCreateBodyDTO = new ComposerCollectionMultipleFoldersAndRequestsCreateBodyDTO
		{
			FolderBeingMoved = ToDTO(folderToCreate),
			Folders = childFolders.Select(ToDTO)
		};
		ComposerCollectionMultipleFoldersAndRequestsCreateBodyDTO composerCollectionMultipleFoldersAndRequestsCreateBodyDTO2 = composerCollectionMultipleFoldersAndRequestsCreateBodyDTO;
		composerCollectionMultipleFoldersAndRequestsCreateBodyDTO2.Requests = await ToDTOAsync(composerCollectionCacheItem.AccountId, childRequests);
		return FromDTO(await fiddlerHttpClient.PatchAsync<ComposerCollectionMultipleFoldersAndRequestsCreateBodyDTO, ComposerCollectionDTO>(resourcePath, composerCollectionMultipleFoldersAndRequestsCreateBodyDTO, validator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken)));
	}

	public async Task<ComposerCollectionRequest> MoveComposerCollectionRequestAsync(Guid requestId, Guid sourceCollectionId, Guid targetCollectionId, Guid? targetFolderId, string concurrencyToken)
	{
		string requestUri = "composer-collections/requests/move";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.BadRequest, HttpStatusCode.PaymentRequired, HttpStatusCode.Forbidden).Create();
		MoveComposerCollectionRequestDTO resource = new MoveComposerCollectionRequestDTO
		{
			RequestId = requestId,
			ComposerCollectionSourceId = sourceCollectionId,
			ComposerCollectionTargetId = targetCollectionId,
			TargetFolderId = targetFolderId
		};
		ComposerCollectionRequestDTO dto = await fiddlerHttpClient.PatchAsync<MoveComposerCollectionRequestDTO, ComposerCollectionRequestDTO>(requestUri, resource, statusCodeValidator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken));
		return FromDTO(dto, await collectionsCache.GetAsync(dto.ComposerCollectionId, GetComposerCollectionCacheItemAsync));
	}

	public async Task<ComposerCollectionRequest> CreateComposerCollectionRequestAsync(Guid composerCollectionId, ComposerCollectionRequest requestToCreate, string concurrencyToken)
	{
		ComposerCollectionCacheItem cacheItem = await collectionsCache.GetAsync(composerCollectionId, GetComposerCollectionCacheItemAsync);
		string resourcePath = string.Format("{0}/{1}/requests", "composer-collections", composerCollectionId);
		IFiddlerHttpStatusCodeValidator validator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.Created).WithErrorCodes(HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.BadRequest, HttpStatusCode.PaymentRequired, HttpStatusCode.Forbidden).Create();
		CreateComposerCollectionRequestDTO createComposerCollectionRequestDTO = await ToDTOAsync(cacheItem.AccountId, requestToCreate);
		createComposerCollectionRequestDTO.ComposerCollectionId = composerCollectionId;
		return FromDTO(await fiddlerHttpClient.PutAsync<CreateComposerCollectionRequestDTO, ComposerCollectionRequestDTO>(resourcePath, createComposerCollectionRequestDTO, validator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken)), cacheItem);
	}

	public async Task DeleteComposerCollectionRequestAsync(Guid composerCollectionId, Guid requestId, string concurrencyToken)
	{
		string requestUri = string.Format("{0}/{1}/requests/{2}", "composer-collections", composerCollectionId, requestId);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.NoContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		await fiddlerHttpClient.DeleteAsync(requestUri, statusCodeValidator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken));
	}

	public async Task<ComposerCollectionRequest> UpdateComposerCollectionRequestAsync(Guid composerCollectionId, Guid requestId, string concurrencyToken, string name = null)
	{
		ComposerCollectionRequest request = new ComposerCollectionRequest
		{
			Id = requestId,
			Name = name
		};
		return await UpdateComposerCollectionRequestDataAsync(composerCollectionId, request, concurrencyToken);
	}

	public async Task<ComposerCollectionRequest> UpdateComposerCollectionRequestAsync(Guid composerCollectionId, ComposerCollectionRequest composerCollectionRequest, string concurrencyToken)
	{
		ComposerCollectionRequest requestWithUpdatedFiles = await UpdateComposerCollectionRequestHeadersAndBodyAsync(composerCollectionId, composerCollectionRequest.Id, composerCollectionRequest.RequestBodyFile, composerCollectionRequest.RequestHeadersFile, concurrencyToken);
		ComposerCollectionRequest requestWithUpdatedMetadata = await UpdateComposerCollectionRequestDataAsync(composerCollectionId, composerCollectionRequest, concurrencyToken);
		ComposerCollectionCacheItem composerCollectionCacheItem = await collectionsCache.GetAsync(composerCollectionId, GetComposerCollectionCacheItemAsync);
		return new ComposerCollectionRequest
		{
			Id = composerCollectionRequest.Id,
			Name = requestWithUpdatedMetadata.Name,
			Description = requestWithUpdatedMetadata.Description,
			Url = requestWithUpdatedMetadata.Url,
			Parameters = requestWithUpdatedMetadata.Parameters,
			HttpMethod = requestWithUpdatedMetadata.HttpMethod,
			HttpVersion = requestWithUpdatedMetadata.HttpVersion,
			ParentId = requestWithUpdatedMetadata.ParentId,
			RequestHeadersFile = requestWithUpdatedFiles.RequestHeadersFile,
			RequestBodyFile = requestWithUpdatedFiles.RequestBodyFile,
			AccountId = composerCollectionCacheItem.AccountId,
			ComposerCollectionId = composerCollectionId,
			Owner = composerCollectionCacheItem.Owner,
			CreatedBy = requestWithUpdatedMetadata.CreatedBy,
			CreatedAt = requestWithUpdatedMetadata.CreatedAt,
			ModifiedAt = requestWithUpdatedMetadata.ModifiedAt,
			ConcurrencyToken = requestWithUpdatedMetadata.ConcurrencyToken
		};
	}

	public async Task<ComposerCollectionRequest> UpdateComposerCollectionRequestHeadersAndBodyAsync(Guid composerCollectionId, Guid requestId, IBlobResource<string> newBodyContent, IBlobResource<string> newHeadersContent, string concurrencyToken)
	{
		ComposerCollectionCacheItem cacheItem = await collectionsCache.GetAsync(composerCollectionId, GetComposerCollectionCacheItemAsync);
		string resourcePath = string.Format("{0}/{1}/requests/{2}/files", "composer-collections", composerCollectionId, requestId);
		IFiddlerHttpStatusCodeValidator validator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PaymentRequired, HttpStatusCode.BadRequest, HttpStatusCode.Conflict, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		UpdateComposerCollectionRequestFilesBodyDTO updateComposerCollectionDTO = new UpdateComposerCollectionRequestFilesBodyDTO();
		if (newBodyContent != null)
		{
			string text = await newBodyContent.GetContentAsStringAsync();
			if (!string.IsNullOrEmpty(text))
			{
				updateComposerCollectionDTO.BodyFileId = await UploadFileToS3Async(cacheItem.AccountId, text);
			}
		}
		if (newHeadersContent != null)
		{
			string text2 = await newHeadersContent.GetContentAsStringAsync();
			if (!string.IsNullOrEmpty(text2))
			{
				updateComposerCollectionDTO.HeadersFileId = await UploadFileToS3Async(cacheItem.AccountId, text2);
			}
		}
		return FromDTO(await fiddlerHttpClient.PostAsync<UpdateComposerCollectionRequestFilesBodyDTO, ComposerCollectionRequestDTO>(resourcePath, updateComposerCollectionDTO, validator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken)), cacheItem);
	}

	public async Task<ComposerCollectionDiff> MoveComposerCollectionFolderAsync(Guid folderId, Guid sourceCollectionId, Guid targetCollectionId, Guid? targetFolderId, string concurrencyToken)
	{
		string requestUri = "composer-collections/folders/move";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.BadRequest, HttpStatusCode.PaymentRequired, HttpStatusCode.Forbidden).Create();
		MoveComposerCollectionFolderDTO resource = new MoveComposerCollectionFolderDTO
		{
			FolderId = folderId,
			ComposerCollectionSourceId = sourceCollectionId,
			ComposerCollectionTargetId = targetCollectionId,
			TargetFolderId = targetFolderId
		};
		ComposerCollectionDiffDTO composerCollectionDiffDTO = await fiddlerHttpClient.PatchAsync<MoveComposerCollectionFolderDTO, ComposerCollectionDiffDTO>(requestUri, resource, statusCodeValidator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken));
		return new ComposerCollectionDiff
		{
			OldCollection = FromDTO(composerCollectionDiffDTO.OldCollection),
			NewCollection = FromDTO(composerCollectionDiffDTO.NewCollection)
		};
	}

	public async Task DeleteComposerCollectionFolderAsync(Guid composerCollectionId, Guid folderId, string concurrencyToken)
	{
		string requestUri = string.Format("{0}/{1}/folders/{2}", "composer-collections", composerCollectionId, folderId);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.NoContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		await fiddlerHttpClient.DeleteAsync(requestUri, statusCodeValidator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken));
	}

	public async Task<ComposerCollectionFolder> UpdateComposerCollectionFolderAsync(Guid composerCollectionId, Guid folderId, string concurrencyToken, string name = null, string description = null)
	{
		string requestUri = string.Format("{0}/{1}/folders/{2}", "composer-collections", composerCollectionId, folderId);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PaymentRequired, HttpStatusCode.BadRequest, HttpStatusCode.Conflict, HttpStatusCode.Unauthorized, HttpStatusCode.PaymentRequired, HttpStatusCode.Forbidden).Create();
		UpdateComposerCollectionFolderBodyDTO resource = new UpdateComposerCollectionFolderBodyDTO
		{
			Name = name,
			Description = description
		};
		return FromDTO(await fiddlerHttpClient.PostAsync<UpdateComposerCollectionFolderBodyDTO, ComposerCollectionFolderDTO>(requestUri, resource, statusCodeValidator, IfMatchHeaderIfConcurrencyEnabled(concurrencyToken)), await collectionsCache.GetAsync(composerCollectionId, GetComposerCollectionCacheItemAsync));
	}

	private static ComposerCollectionFolderDTO ToDTO(ComposerCollectionFolder folder)
	{
		return new ComposerCollectionFolderDTO
		{
			Id = folder.Id,
			Name = folder.Name,
			Description = folder.Description,
			ParentId = folder.ParentId,
			CreatedAt = folder.CreatedAt,
			ModifiedAt = folder.ModifiedAt,
			CreatedBy = folder.CreatedBy
		};
	}

	private static ComposerCollectionFolder FromDTO(ComposerCollectionFolderDTO dto, ComposerCollectionCacheItem composerCollection)
	{
		return new ComposerCollectionFolder
		{
			Id = dto.Id,
			ParentId = dto.ParentId,
			Name = dto.Name,
			Description = dto.Description,
			ComposerCollectionId = composerCollection.Id,
			AccountId = composerCollection.AccountId,
			Owner = composerCollection.Owner,
			CreatedBy = dto.CreatedBy,
			CreatedAt = dto.CreatedAt,
			ModifiedAt = dto.ModifiedAt,
			ConcurrencyToken = dto.ConcurrencyToken
		};
	}

	private IList<byte[]> GetFileChunks(byte[] bodyContentBytes)
	{
		using Stream stream = new MemoryStream(bodyContentBytes);
		byte[] array = new byte[fileClient.ChunkSize];
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

	private async Task<ComposerCollectionDTO> GetComposerCollectionDTOAsync(Guid composerCollectionId)
	{
		string requestUri = string.Format("{0}/{1}", "composer-collections", composerCollectionId);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		return await fiddlerHttpClient.GetAsync<ComposerCollectionDTO>(requestUri, statusCodeValidator);
	}

	private async Task<ComposerCollectionCacheItem> GetComposerCollectionCacheItemAsync(Guid composerCollectionId)
	{
		ComposerCollectionDTO composerCollectionDTO = await GetComposerCollectionDTOAsync(composerCollectionId);
		return ((IMapperBase)mapper).Map<ComposerCollectionCacheItem>((object)composerCollectionDTO);
	}

	private async Task<CreateComposerCollectionDTO> ToDTOAsync(Guid accountId, ComposerCollection composerCollection)
	{
		CreateComposerCollectionDTO createComposerCollectionDTO = new CreateComposerCollectionDTO
		{
			Id = composerCollection.Id,
			AccountId = accountId,
			Version = composerCollection.Version,
			Name = composerCollection.Name,
			CreatedAt = composerCollection.CreatedAt,
			Description = composerCollection.Description,
			Folders = composerCollection.Folders.Select((ComposerCollectionFolder x) => new ComposerCollectionFolderDTO
			{
				Id = x.Id,
				Name = x.Name,
				Description = x.Description,
				ParentId = x.ParentId,
				CreatedAt = x.CreatedAt,
				ModifiedAt = x.ModifiedAt,
				CreatedBy = x.CreatedBy
			}).ToList()
		};
		foreach (ComposerCollectionRequest request in composerCollection.Requests)
		{
			CreateComposerCollectionRequestDTO item = await ToDTOAsync(accountId, request);
			createComposerCollectionDTO.Requests.Add(item);
		}
		return createComposerCollectionDTO;
	}

	private async Task<IEnumerable<CreateComposerCollectionRequestDTO>> ToDTOAsync(Guid accountId, IEnumerable<ComposerCollectionRequest> requests)
	{
		List<CreateComposerCollectionRequestDTO> res = new List<CreateComposerCollectionRequestDTO>();
		foreach (ComposerCollectionRequest request in requests)
		{
			res.Add(await ToDTOAsync(accountId, request));
		}
		return res;
	}

	private async Task<CreateComposerCollectionRequestDTO> ToDTOAsync(Guid accountId, ComposerCollectionRequest request)
	{
		CreateComposerCollectionRequestDTO createComposerCollectionRequestDTO = new CreateComposerCollectionRequestDTO
		{
			Id = request.Id,
			Name = request.Name,
			Description = request.Description,
			Url = request.Url,
			Parameters = request.Parameters,
			HttpMethod = request.HttpMethod,
			HttpVersion = request.HttpVersion,
			ParentId = request.ParentId,
			CreatedAt = request.CreatedAt
		};
		CreateComposerCollectionRequestDTO createComposerCollectionRequestDTO2 = createComposerCollectionRequestDTO;
		createComposerCollectionRequestDTO2.RequestBodyFileId = await UploadBlobIfNotNull(accountId, request.RequestBodyFile);
		CreateComposerCollectionRequestDTO createComposerCollectionRequestDTO3 = createComposerCollectionRequestDTO;
		createComposerCollectionRequestDTO3.RequestHeadersFileId = await UploadBlobIfNotNull(accountId, request.RequestHeadersFile);
		return createComposerCollectionRequestDTO;
	}

	private ComposerCollection FromDTO(ComposerCollectionDTO dto)
	{
		List<ComposerCollectionFolder> folders = dto.Folders.Select((ComposerCollectionFolderDTO x) => new ComposerCollectionFolder
		{
			Id = x.Id,
			ParentId = x.ParentId,
			Name = x.Name,
			Description = x.Description,
			ComposerCollectionId = x.ComposerCollectionId,
			CreatedAt = x.CreatedAt,
			ModifiedAt = x.ModifiedAt,
			AccountId = dto.AccountId,
			Owner = dto.Owner,
			CreatedBy = x.CreatedBy,
			ConcurrencyToken = x.ConcurrencyToken
		}).ToList();
		List<ComposerCollectionRequest> requests = dto.Requests.Select((ComposerCollectionRequestDTO x) => FromDTO(x, ((IMapperBase)mapper).Map<ComposerCollectionCacheItem>((object)dto))).ToList();
		return new ComposerCollection
		{
			Id = dto.Id,
			AccountId = dto.AccountId,
			Owner = dto.Owner,
			CreatedAt = dto.CreatedAt,
			ModifiedAt = dto.ModifiedAt,
			Folders = folders,
			Requests = requests,
			Version = dto.Version,
			Name = dto.Name,
			Description = dto.Description,
			ConcurrencyToken = dto.ConcurrencyToken,
			SharedWith = dto.SharedWith.Select((ShareDTO x) => new ComposerCollectionShareReceiver
			{
				Email = x.Email,
				Reason = x.Note
			})
		};
	}

	private ComposerCollectionRequest FromDTO(ComposerCollectionRequestDTO dto, ComposerCollectionCacheItem composerCollection)
	{
		ComposerCollectionRequest composerCollectionRequest = new ComposerCollectionRequest
		{
			Id = dto.Id,
			ParentId = dto.ParentId,
			Url = dto.Url,
			Parameters = dto.Parameters,
			HttpMethod = dto.HttpMethod,
			HttpVersion = dto.HttpVersion,
			Name = dto.Name,
			Description = dto.Description,
			ComposerCollectionId = dto.ComposerCollectionId,
			AccountId = composerCollection.AccountId,
			Owner = composerCollection.Owner,
			CreatedBy = dto.CreatedBy,
			ConcurrencyToken = dto.ConcurrencyToken,
			CreatedAt = dto.CreatedAt,
			ModifiedAt = dto.ModifiedAt
		};
		FileDTO requestHeadersFile = dto.RequestHeadersFile;
		if (requestHeadersFile != null)
		{
			_ = requestHeadersFile.Id;
			if (true)
			{
				composerCollectionRequest.RequestHeadersFile = new RemoteBlobResource<string>(dto.RequestHeadersFile.Id, DownloadFileAsync);
				goto IL_0109;
			}
		}
		composerCollectionRequest.RequestHeadersFile = new LocalBlobResource<string>(string.Empty);
		goto IL_0109;
		IL_0109:
		FileDTO requestBodyFile = dto.RequestBodyFile;
		if (requestBodyFile != null)
		{
			_ = requestBodyFile.Id;
			if (true)
			{
				composerCollectionRequest.RequestBodyFile = new RemoteBlobResource<string>(dto.RequestBodyFile.Id, DownloadFileAsync);
				goto IL_0158;
			}
		}
		composerCollectionRequest.RequestBodyFile = new LocalBlobResource<string>(string.Empty);
		goto IL_0158;
		IL_0158:
		return composerCollectionRequest;
	}

	private async Task<Guid?> UploadBlobIfNotNull<T>(Guid accountId, IBlobResource<T> blob)
	{
		if (blob == null)
		{
			return null;
		}
		string text = await blob.GetContentAsStringAsync();
		if (string.IsNullOrEmpty(text))
		{
			return null;
		}
		return await UploadFileToS3Async(accountId, text);
	}

	private async Task<Guid> UploadFileToS3Async(Guid accountId, string bodyContent)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(bodyContent);
		List<byte[]> source = GetFileChunks(bytes).ToList();
		string chunkChecksums = string.Join(';', source.Select((byte[] chunk) => md5Calculator.Calculate(chunk)));
		Dictionary<string, string> uploadHeaders = GetUploadHeaders(bytes, chunkChecksums);
		using MemoryStream inputStream = new MemoryStream(bytes);
		InitiateMultipartUploadResponseDTO multipartUploadResponse = await fileClient.InitiateMultipartFileUploadAsync(accountId, uploadHeaders);
		IEnumerable<PartETagDTO> etags = await fileClient.UploadToS3Async(inputStream, multipartUploadResponse.UploadUrls.ToList());
		Guid fileId = multipartUploadResponse.FileId;
		await fileClient.CompleteFileUploadAsync(accountId, fileId, etags);
		return fileId;
	}

	private Dictionary<string, string> GetUploadHeaders(byte[] bodyContentBytes, string chunkChecksums)
	{
		string value = md5Calculator.Calculate(bodyContentBytes);
		return new Dictionary<string, string>
		{
			{ "X-Upload-Content-Type", "application/octet-stream" },
			{
				"X-Upload-Content-Length",
				bodyContentBytes.Length.ToString()
			},
			{ "X-Upload-Content-MD5", value },
			{
				"X-Upload-Chunk-Size",
				fileClient.ChunkSize.ToString()
			},
			{ "X-Upload-Chunk-Checksums", chunkChecksums }
		};
	}

	private async Task<string> DownloadFileAsync(Guid? remoteFileId)
	{
		if (!remoteFileId.HasValue)
		{
			return string.Empty;
		}
		string requestAbsoluteUri = await fileClient.GetFileUrlAsync(remoteFileId.Value);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCodes(HttpStatusCode.OK, HttpStatusCode.PartialContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
		using HttpResponseMessage httpResponseMessage = await fiddlerHttpClient.GetExternalAsync(requestAbsoluteUri, statusCodeValidator, HttpCompletionOption.ResponseHeadersRead);
		return await httpResponseMessage.Content.ReadAsStringAsync();
	}

	private async Task<ComposerCollectionRequest> UpdateComposerCollectionRequestDataAsync(Guid composerCollectionId, ComposerCollectionRequest request, string concurrencyToken)
	{
		string requestUri = string.Format("{0}/{1}/requests/{2}", "composer-collections", composerCollectionId, request.Id);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PaymentRequired, HttpStatusCode.BadRequest, HttpStatusCode.Conflict, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		UpdateComposerCollectionRequestBodyDTO resource = new UpdateComposerCollectionRequestBodyDTO
		{
			Name = request.Name,
			Description = request.Description,
			Parameters = request.Parameters,
			HttpMethod = request.HttpMethod,
			HttpVersion = request.HttpVersion,
			Url = request.Url,
			ConcurrencyToken = concurrencyToken
		};
		return FromDTO(await fiddlerHttpClient.PostAsync<UpdateComposerCollectionRequestBodyDTO, ComposerCollectionRequestDTO>(requestUri, resource, statusCodeValidator), await collectionsCache.GetAsync(composerCollectionId, GetComposerCollectionCacheItemAsync));
	}

	private IEnumerable<(string, string)> IfMatchHeaderIfConcurrencyEnabled(string concurrencyToken)
	{
		if (!backendConfiguration.DisableConcurrency)
		{
			return new List<(string, string)> { ("If-Match", concurrencyToken) };
		}
		return new List<(string, string)>();
	}
}
