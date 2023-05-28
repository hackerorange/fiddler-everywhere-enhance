using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FiddlerBackend.Contracts;
using FiddlerBackend.Contracts.DTO.Snapshots;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Files.Client;
using Ionic.Zip;

namespace FiddlerBackendSDK.Snapshot.Client;

public class SnapshotClient : ISnapshotClient
{
	private readonly IFiddlerHttpClient fiddlerHttpClient;

	private readonly IFileClient fileClient;

	private readonly string snapshotsRelativePath = "snapshots";

	private readonly string snapshotRequestCommentsRelativePath = "snapshot-request-comments";

	private readonly IFiddlerHttpStatusCodeValidator getSnapshotListStatusCodeValidator;

	private readonly ISnapshotRepository snapshotRepository;

	private readonly IMD5Calculator mD5Calculator;

	private readonly IValidationExceptionFactory exceptionFactory;

	private readonly IBackendConfiguration backendConfiguration;

	private readonly IMapper mapper;

	private readonly IFileCache fileCache;

	public SnapshotClient(IFiddlerHttpClient fiddlerHttpClient, IFileClient fileClient, ISnapshotRepository snapshotRepository, IMD5Calculator mD5Calculator, IValidationExceptionFactory exceptionFactory, IBackendConfiguration backendConfiguration, IMapper mapper, IFileCache fileCache)
	{
		this.fiddlerHttpClient = fiddlerHttpClient;
		this.fileClient = fileClient;
		this.snapshotRepository = snapshotRepository;
		this.mD5Calculator = mD5Calculator;
		this.exceptionFactory = exceptionFactory;
		this.backendConfiguration = backendConfiguration;
		this.mapper = mapper;
		this.fileCache = fileCache;
		getSnapshotListStatusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(this.exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
	}

	public async Task<SnapshotMetadata> GetSnapshotAsync(Guid snapshotId, bool includeDeleted = true)
	{
		string requestUri = $"{snapshotsRelativePath}/{snapshotId}?includeDeleted={includeDeleted}";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		SnapshotDTO snapshotDTO = await fiddlerHttpClient.GetAsync<SnapshotDTO>(requestUri, statusCodeValidator);
		if (snapshotDTO.IsDeleted)
		{
			RemoveSnapshotFromCache(snapshotDTO.Id);
		}
		return ((IMapperBase)mapper).Map<SnapshotMetadata>((object)snapshotDTO);
	}

	public async Task<IList<SnapshotMetadata>> GetOwnSnapshotsAsync(bool includeDeleted = true)
	{
		string requestUri = $"{snapshotsRelativePath}/mine?includeDeleted={includeDeleted}";
		IEnumerable<SnapshotDTO> enumerable = await fiddlerHttpClient.GetAsync<IEnumerable<SnapshotDTO>>(requestUri, getSnapshotListStatusCodeValidator);
		RemoveDeletedFromCache(enumerable);
		return ((IMapperBase)mapper).Map<List<SnapshotMetadata>>((object)enumerable);
	}

	public async Task<IList<SnapshotMetadata>> GetSnapshotsSharedWithMeAsync(bool includeDeleted = true)
	{
		string requestUri = $"{snapshotsRelativePath}/shared-with-me?includeDeleted={includeDeleted}";
		IEnumerable<SnapshotDTO> enumerable = await fiddlerHttpClient.GetAsync<IEnumerable<SnapshotDTO>>(requestUri, getSnapshotListStatusCodeValidator);
		RemoveDeletedFromCache(enumerable);
		return ((IMapperBase)mapper).Map<List<SnapshotMetadata>>((object)enumerable);
	}

	public Task<IList<SnapshotMetadata>> GetAvailableSnapshotsAsync(bool includeDeleted = true)
	{
		string text = "123456";
		return Task.FromResult((IList<SnapshotMetadata>)new List<SnapshotMetadata>());
	}

	public async Task<SnapshotMetadata> UploadSnapshotAsync(Guid accountId, Guid snapshotId, string filePath, string snapshotName, string snapshotDescription = null, IEnumerable<SnapshotRequestComment> comments = null, bool isPasswordProtected = false)
	{
		Guid fileId = await UploadFileToS3Async(accountId, filePath);
		string requestUri = snapshotsRelativePath ?? "";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.Created).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PaymentRequired, HttpStatusCode.BadRequest, HttpStatusCode.Conflict, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		IEnumerable<SnapshotRequestCommentDTO> comments2 = ((IMapperBase)mapper).Map<IEnumerable<SnapshotRequestCommentDTO>>((object)(comments ?? new List<SnapshotRequestComment>()));
		CreateSnapshotDTO resource = new CreateSnapshotDTO
		{
			Id = snapshotId,
			AccountId = accountId,
			FileId = fileId,
			Name = snapshotName,
			Description = snapshotDescription,
			Comments = comments2,
			IsPasswordProtected = isPasswordProtected
		};
		Task<SnapshotDTO> task = fiddlerHttpClient.PutAsync<CreateSnapshotDTO, SnapshotDTO>(requestUri, resource, statusCodeValidator);
		snapshotRepository.AddFileToCache(snapshotId, fileId, filePath);
		IMapper val = mapper;
		return ((IMapperBase)val).Map<SnapshotMetadata>((object)(await task));
	}

	public async Task<SnapshotMetadata> UpdateSnapshotNameAsync(Guid snapshotId, string name, string concurrencyToken)
	{
		return await UpdateSnapshotNameAndDescriptionAsync(snapshotId, name, null, concurrencyToken);
	}

	public async Task<SnapshotMetadata> UpdateSnapshotNameAndDescriptionAsync(Guid snapshotId, string name, string description, string concurrencyToken)
	{
		string requestUri = $"{snapshotsRelativePath}/{snapshotId}";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCodes(HttpStatusCode.OK, HttpStatusCode.NoContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PreconditionFailed, HttpStatusCode.Unauthorized, HttpStatusCode.BadRequest, HttpStatusCode.PaymentRequired, HttpStatusCode.Forbidden).Create();
		UpdateSnapshotDTO resource = new UpdateSnapshotDTO
		{
			Name = name,
			Description = description
		};
		SnapshotDTO snapshotDTO = await fiddlerHttpClient.PatchAsync<UpdateSnapshotDTO, SnapshotDTO>(requestUri, resource, statusCodeValidator, CreateModificationRequestHeaders(concurrencyToken));
		return ((IMapperBase)mapper).Map<SnapshotMetadata>((object)snapshotDTO);
	}

	public async Task<SnapshotMetadata> UpdateSnapshotEmailsAsync(Guid snapshotId, IEnumerable<string> newEmails, string reason, string concurrencyToken)
	{
		string requestUri = $"{snapshotsRelativePath}/{snapshotId}/shares";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCodes(HttpStatusCode.Created, HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized, HttpStatusCode.PaymentRequired, HttpStatusCode.PreconditionFailed, HttpStatusCode.Forbidden).Create();
		List<ShareDTO> resource = newEmails.Select((string email) => new ShareDTO
		{
			Email = email,
			Note = reason
		}).ToList();
		SnapshotDTO snapshotDTO = await fiddlerHttpClient.PostAsync<IEnumerable<ShareDTO>, SnapshotDTO>(requestUri, resource, statusCodeValidator, CreateModificationRequestHeaders(concurrencyToken));
		return ((IMapperBase)mapper).Map<SnapshotMetadata>((object)snapshotDTO);
	}

	public async Task DeleteSnapshotAsync(Guid snapshotId, string concurrencyToken)
	{
		string requestUri = $"{snapshotsRelativePath}/{snapshotId}";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.NoContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.PreconditionFailed, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		await fiddlerHttpClient.DeleteAsync(requestUri, statusCodeValidator, CreateModificationRequestHeaders(concurrencyToken));
		RemoveSnapshotFromCache(snapshotId);
	}

	public async Task DownloadSnapshotAsync(Guid snapshotId, string filePath, string password = null)
	{
		SnapshotMetadata snapshotMetadata = await GetSnapshotAsync(snapshotId, includeDeleted: false);
		SnapshotVersionFileMetadata currentFile = snapshotMetadata.SnapshotFileVersions.OrderByDescending((SnapshotVersionFileMetadata x) => x.CreatedAt).FirstOrDefault();
		await GetSnapshotFileAsync(snapshotMetadata, currentFile, filePath, password);
	}

	public async Task<SnapshotMetadata> UpdateSnapshotFileAsync(Guid snapshotId, string filePath, string concurrencyToken, string password = null)
	{
		return await UploadNewSnapshotVersionAsync(await GetSnapshotAsync(snapshotId, includeDeleted: false), filePath, concurrencyToken, password);
	}

	public async Task<IEnumerable<SnapshotRequestComment>> GetAllCommentsForSnapshotAsync(Guid snapshotId)
	{
		string requestUri = $"{snapshotsRelativePath}/{snapshotId}/comments";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden, HttpStatusCode.BadRequest).Create();
		IEnumerable<SnapshotRequestCommentDTO> enumerable = await fiddlerHttpClient.GetAsync<IEnumerable<SnapshotRequestCommentDTO>>(requestUri, statusCodeValidator);
		return ((IMapperBase)mapper).Map<IEnumerable<SnapshotRequestComment>>((object)enumerable);
	}

	public async Task<SnapshotRequestComment> CreateCommentOrReplyAsync(Guid snapshotId, Guid snapshotRequestId, string text, Guid? parentCommentId = null)
	{
		string requestUri = $"{snapshotsRelativePath}/{snapshotId}/requests/{snapshotRequestId}/comments";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden, HttpStatusCode.BadRequest).Create();
		RequestCommentBodyDTO resource = new RequestCommentBodyDTO
		{
			Text = text,
			ParentCommentId = parentCommentId
		};
		SnapshotRequestCommentDTO snapshotRequestCommentDTO = await fiddlerHttpClient.PostAsync<RequestCommentBodyDTO, SnapshotRequestCommentDTO>(requestUri, resource, statusCodeValidator);
		return ((IMapperBase)mapper).Map<SnapshotRequestComment>((object)snapshotRequestCommentDTO);
	}

	public async Task<SnapshotRequestComment> UpdateCommentOrReplyAsync(Guid commentOrReplyId, string text)
	{
		string requestUri = $"{snapshotRequestCommentsRelativePath}/{commentOrReplyId}";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden, HttpStatusCode.BadRequest).Create();
		RequestCommentBodyDTO resource = new RequestCommentBodyDTO
		{
			Text = text
		};
		SnapshotRequestCommentDTO snapshotRequestCommentDTO = await fiddlerHttpClient.PostAsync<RequestCommentBodyDTO, SnapshotRequestCommentDTO>(requestUri, resource, statusCodeValidator);
		return ((IMapperBase)mapper).Map<SnapshotRequestComment>((object)snapshotRequestCommentDTO);
	}

	public async Task DeleteCommentOrReplyAsync(Guid commentOrReplyId)
	{
		string requestUri = $"{snapshotRequestCommentsRelativePath}/{commentOrReplyId}";
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.NoContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden).Create();
		await fiddlerHttpClient.DeleteAsync(requestUri, statusCodeValidator);
	}

	public async Task<SnapshotMetadata> UpdateSnapshotPasswordAsync(Guid snapshotId, string oldPassword, string newPassword, string targetPath = null)
	{
		_ = 2;
		try
		{
			SnapshotMetadata snapshot = await GetSnapshotAsync(snapshotId, includeDeleted: false);
			if (oldPassword == newPassword)
			{
				return snapshot;
			}
			SnapshotVersionFileMetadata currentFile = snapshot.SnapshotFileVersions.OrderByDescending((SnapshotVersionFileMetadata x) => x.CreatedAt).FirstOrDefault();
			using FileStream fileStream = await GetSnapshotFileAsync(snapshot, currentFile, oldPassword);
			using MemoryStream memoryStream = ChangeArchivePassword(fileStream, oldPassword, newPassword);
			if (targetPath != null)
			{
				using FileStream destination = File.OpenWrite(targetPath);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				memoryStream.CopyTo(destination);
			}
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return await UploadNewSnapshotVersionNoDeltaAsync(snapshot, memoryStream, null, !string.IsNullOrEmpty(newPassword), isPasswordUpdated: true);
		}
		catch (BadPasswordException val)
		{
			throw new WrongPasswordException(((Exception)val).Message);
		}
	}

	private MemoryStream ChangeArchivePassword(FileStream fileStream, string oldPassword, string newPassword)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		ZipFile val = ZipFile.Read((Stream)fileStream);
		try
		{
			ZipFile val2 = new ZipFile();
			try
			{
				if (!string.IsNullOrEmpty(oldPassword))
				{
					val.Password = oldPassword;
					val.Encryption = (EncryptionAlgorithm)3;
				}
				if (!string.IsNullOrEmpty(newPassword))
				{
					val2.Password = newPassword;
					val2.Encryption = (EncryptionAlgorithm)3;
				}
				foreach (ZipEntry entry in val.Entries)
				{
					using MemoryStream memoryStream = new MemoryStream();
					if (string.IsNullOrEmpty(oldPassword))
					{
						entry.Extract((Stream)memoryStream);
					}
					else
					{
						entry.ExtractWithPassword((Stream)memoryStream, oldPassword);
					}
					byte[] array = memoryStream.ToArray();
					val2.AddEntry(entry.FileName, array);
				}
				MemoryStream memoryStream2 = new MemoryStream();
				val2.Save((Stream)memoryStream2);
				return memoryStream2;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private void RemoveDeletedFromCache(IEnumerable<SnapshotDTO> snapshotsToCheck)
	{
		snapshotsToCheck.Where((SnapshotDTO x) => x.IsDeleted).ToList().ForEach(delegate(SnapshotDTO x)
		{
			RemoveSnapshotFromCache(x.Id);
		});
	}

	private void RemoveSnapshotFromCache(Guid snapshotId)
	{
		string cachePath = $"{snapshotsRelativePath}/{snapshotId}";
		fileCache.DeletePath(cachePath);
	}

	private IEnumerable<(string, string)> CreateModificationRequestHeaders(string concurrencyToken)
	{
		if (!backendConfiguration.DisableConcurrency)
		{
			return new List<(string, string)> { ("If-Match", concurrencyToken) };
		}
		return new List<(string, string)>();
	}

	private async Task<Guid> UploadFileToS3Async(Guid accountId, string filePath)
	{
		string chunkChecksums = string.Join(';', GetFileChecksums(filePath));
		Dictionary<string, string> uploadHeaders = GetUploadHeaders(filePath, chunkChecksums);
		using FileStream fileStream = File.OpenRead(filePath);
		InitiateMultipartUploadResponseDTO multipartUploadResponse = await fileClient.InitiateMultipartFileUploadAsync(accountId, uploadHeaders);
		IEnumerable<PartETagDTO> etags = await fileClient.UploadToS3Async(fileStream, multipartUploadResponse.UploadUrls.ToList());
		Guid fileId = multipartUploadResponse.FileId;
		await fileClient.CompleteFileUploadAsync(accountId, fileId, etags);
		return fileId;
	}

	private async Task<Guid> UploadFileToS3Async(Guid accountId, MemoryStream fileStream)
	{
		string chunkChecksums = string.Join(';', GetFileChecksums(fileStream));
		Dictionary<string, string> uploadHeaders = GetUploadHeaders(fileStream, chunkChecksums);
		InitiateMultipartUploadResponseDTO multipartUploadResponse = await fileClient.InitiateMultipartFileUploadAsync(accountId, uploadHeaders);
		IEnumerable<PartETagDTO> etags = await fileClient.UploadToS3Async(fileStream, multipartUploadResponse.UploadUrls.ToList());
		Guid fileId = multipartUploadResponse.FileId;
		await fileClient.CompleteFileUploadAsync(accountId, fileId, etags);
		return fileId;
	}

	private Dictionary<string, string> GetUploadHeaders(string filePath, string chunkChecksums)
	{
		long length = new FileInfo(filePath).Length;
		string value = mD5Calculator.Calculate(filePath);
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
				fileClient.ChunkSize.ToString()
			},
			{ "X-Upload-Chunk-Checksums", chunkChecksums }
		};
	}

	private Dictionary<string, string> GetUploadHeaders(MemoryStream fileStream, string chunkChecksums)
	{
		long length = fileStream.Length;
		string value = mD5Calculator.Calculate(fileStream);
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
				fileClient.ChunkSize.ToString()
			},
			{ "X-Upload-Chunk-Checksums", chunkChecksums }
		};
	}

	private IList<string> GetFileChecksums(string filePath)
	{
		using FileStream stream = File.OpenRead(filePath);
		return GetFileChecksums(stream);
	}

	private IList<string> GetFileChecksums(Stream stream)
	{
		byte[] array = new byte[fileClient.ChunkSize];
		List<string> list = new List<string>();
		int num;
		while ((num = stream.Read(array, 0, array.Length)) > 0)
		{
			byte[] array2 = new byte[num];
			Buffer.BlockCopy(array, 0, array2, 0, num);
			string item = mD5Calculator.Calculate(array2);
			list.Add(item);
		}
		return list;
	}

	private async Task<SnapshotMetadata> UploadNewSnapshotVersionAsync(SnapshotMetadata snapshot, string filePath, string concurrencyToken, string password)
	{
		_ = 2;
		try
		{
			string deltaVersionFilePath = await snapshotRepository.GetTempVersionFileDeltaAsync(snapshot, filePath, password);
			Guid fileId = await UploadFileToS3Async(snapshot.AccountId, deltaVersionFilePath);
			string requestUri = $"{snapshotsRelativePath}/{snapshot.Id}/versions";
			IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Conflict, HttpStatusCode.PreconditionFailed, HttpStatusCode.Unauthorized, HttpStatusCode.PaymentRequired, HttpStatusCode.Forbidden).Create();
			AddSnapshotVersionDTO resource = new AddSnapshotVersionDTO
			{
				FileId = fileId,
				IsDelta = true,
				IsPasswordProtected = !string.IsNullOrEmpty(password)
			};
			Task<SnapshotDTO> task = fiddlerHttpClient.PutAsync<AddSnapshotVersionDTO, SnapshotDTO>(requestUri, resource, statusCodeValidator, CreateModificationRequestHeaders(concurrencyToken));
			snapshotRepository.AddFileToCache(snapshot.Id, fileId, deltaVersionFilePath, deleteOriginal: true);
			IMapper val = mapper;
			return ((IMapperBase)val).Map<SnapshotMetadata>((object)(await task));
		}
		catch (BadPasswordException val2)
		{
			throw new WrongPasswordException(((Exception)val2).Message);
		}
	}

	private async Task<SnapshotMetadata> UploadNewSnapshotVersionNoDeltaAsync(SnapshotMetadata snapshot, MemoryStream fileStream, string concurrencyToken, bool isPasswordProtected = false, bool isPasswordUpdated = false)
	{
		_ = 2;
		try
		{
			Guid fileId = await UploadFileToS3Async(snapshot.AccountId, fileStream);
			string requestUri = $"{snapshotsRelativePath}/{snapshot.Id}/versions";
			IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Conflict, HttpStatusCode.PreconditionFailed, HttpStatusCode.Unauthorized, HttpStatusCode.PaymentRequired, HttpStatusCode.Forbidden).Create();
			AddSnapshotVersionDTO resource = new AddSnapshotVersionDTO
			{
				FileId = fileId,
				IsDelta = false,
				IsPasswordProtected = isPasswordProtected,
				IsPasswordUpdated = isPasswordUpdated
			};
			Task<SnapshotDTO> newSnapshotVersionTask = fiddlerHttpClient.PutAsync<AddSnapshotVersionDTO, SnapshotDTO>(requestUri, resource, statusCodeValidator, CreateModificationRequestHeaders(concurrencyToken));
			fileStream.Seek(0L, SeekOrigin.Begin);
			await snapshotRepository.AddFileToCache(snapshot.Id, fileId, fileStream);
			IMapper val = mapper;
			return ((IMapperBase)val).Map<SnapshotMetadata>((object)(await newSnapshotVersionTask));
		}
		catch (BadPasswordException val2)
		{
			throw new WrongPasswordException(((Exception)val2).Message);
		}
	}

	private async Task GetSnapshotFileAsync(SnapshotMetadata snapshot, SnapshotVersionFileMetadata currentFile, string filePath, string password)
	{
		_ = 1;
		try
		{
			string sourceFileName = ((currentFile != null) ? (await snapshotRepository.GetVersionFileLocationAsync(snapshot, currentFile, password)) : (await snapshotRepository.GetFileLocationAsync(snapshot, snapshot.SnapshotFile)));
			File.Copy(sourceFileName, filePath);
		}
		catch (BadPasswordException val)
		{
			throw new WrongPasswordException(((Exception)val).Message);
		}
	}

	private async Task<FileStream> GetSnapshotFileAsync(SnapshotMetadata snapshot, SnapshotVersionFileMetadata currentFile, string password)
	{
		_ = 1;
		try
		{
			string path = ((currentFile != null) ? (await snapshotRepository.GetVersionFileLocationAsync(snapshot, currentFile, password)) : (await snapshotRepository.GetFileLocationAsync(snapshot, snapshot.SnapshotFile)));
			return File.OpenRead(path);
		}
		catch (BadPasswordException val)
		{
			throw new WrongPasswordException(((Exception)val).Message);
		}
	}
}
