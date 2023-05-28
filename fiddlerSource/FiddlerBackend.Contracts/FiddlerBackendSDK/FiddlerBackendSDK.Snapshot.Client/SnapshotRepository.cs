using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.Snapshot.Client;

public class SnapshotRepository : ISnapshotRepository
{
	private readonly IFileClient fileClient;

	private readonly string snapshotsPath = "snapshots";

	private readonly IFileCache fileCache;

	private readonly ISnapshotTransformer snapshotTransformer;

	private readonly IMD5Calculator mD5Calculator;

	private readonly IValidationExceptionFactory exceptionFactory;

	private readonly IFileDownloader fileDownloader;

	public SnapshotRepository(IFileClient fileClient, IFileCache fileCache, ISnapshotTransformer snapshotTransformer, IMD5Calculator mD5Calculator, IValidationExceptionFactory validationExceptionFactory, IFileDownloader fileDownloader)
	{
		this.fileClient = fileClient;
		this.fileCache = fileCache;
		this.snapshotTransformer = snapshotTransformer;
		this.mD5Calculator = mD5Calculator;
		exceptionFactory = validationExceptionFactory;
		this.fileDownloader = fileDownloader;
	}

	public async Task<string> GetFileLocationAsync(SnapshotMetadata snapshot, RemoteFileMetadata snapshotRemoteFile)
	{
		string cachePath = Path.Combine(snapshotsPath, snapshot.Id.ToString(), snapshotRemoteFile.Id.ToString());
		string fileLocation = fileCache.GetFileLocation(cachePath);
		if (fileLocation != null)
		{
			if (string.IsNullOrWhiteSpace(snapshotRemoteFile.ContentMD5))
			{
				return fileLocation;
			}
			if (!(mD5Calculator.Calculate(fileLocation) != snapshotRemoteFile.ContentMD5))
			{
				return fileLocation;
			}
			fileCache.DeletePath(cachePath);
		}
		string fileUrl = await fileClient.GetFileUrlAsync(snapshotRemoteFile.Id);
		string targetPath = fileCache.GetTargetPath(cachePath);
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCodes(HttpStatusCode.OK, HttpStatusCode.PartialContent).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
		await fileDownloader.DownloadFileAsync(fileUrl, targetPath, statusCodeValidator);
		return targetPath;
	}

	public async Task<string> GetVersionFileLocationAsync(SnapshotMetadata snapshot, SnapshotVersionFileMetadata versionFile, string password)
	{
		string versionFileLocation = await GetFileLocationAsync(snapshot, versionFile);
		if (!versionFile.IsDelta)
		{
			return versionFileLocation;
		}
		string cacheFullVersionFilePath = Path.Combine(snapshotsPath, snapshot.Id.ToString(), $"{snapshot.SnapshotFile.Id}-{versionFile.Id}");
		string fileLocation = fileCache.GetFileLocation(cacheFullVersionFilePath);
		if (fileLocation != null)
		{
			return fileLocation;
		}
		SnapshotVersionFileMetadata snapshotVersionFileMetadata = (from x in snapshot.SnapshotFileVersions?.Where((SnapshotVersionFileMetadata x) => !x.IsDelta)
			orderby x.CreatedAt descending
			select x).FirstOrDefault();
		string text = ((snapshotVersionFileMetadata != null) ? (await GetFileLocationAsync(snapshot, snapshotVersionFileMetadata)) : (await GetFileLocationAsync(snapshot, snapshot.SnapshotFile)));
		string mainFilePath = text;
		using Stream fileStream = await snapshotTransformer.ApplyDeltaAsync(mainFilePath, versionFileLocation, password);
		return await fileCache.SaveFileAsync(cacheFullVersionFilePath, fileStream);
	}

	public async Task<string> GetTempVersionFileDeltaAsync(SnapshotMetadata snapshot, string versionFile, string password)
	{
		SnapshotVersionFileMetadata snapshotVersionFileMetadata = (from x in snapshot.SnapshotFileVersions?.Where((SnapshotVersionFileMetadata x) => !x.IsDelta)
			orderby x.CreatedAt descending
			select x).FirstOrDefault();
		string text = ((snapshotVersionFileMetadata != null) ? (await GetFileLocationAsync(snapshot, snapshotVersionFileMetadata)) : (await GetFileLocationAsync(snapshot, snapshot.SnapshotFile)));
		string mainFilePath = text;
		string cacheFullVersionFilePath = Path.Combine(snapshotsPath, snapshot.Id.ToString(), $"temp-{Guid.NewGuid()}");
		using Stream fileStream = await snapshotTransformer.CreateDeltaAsync(mainFilePath, versionFile, password);
		return await fileCache.SaveFileAsync(cacheFullVersionFilePath, fileStream);
	}

	public string AddFileToCache(Guid snapshotId, Guid versionFileId, string filePath, bool deleteOriginal = false)
	{
		string cachePath = Path.Combine(snapshotsPath, snapshotId.ToString(), versionFileId.ToString());
		return fileCache.SaveFile(cachePath, filePath, deleteOriginal);
	}

	public async Task<string> AddFileToCache(Guid snapshotId, Guid versionFileId, Stream fileStream)
	{
		string cachePath = Path.Combine(snapshotsPath, snapshotId.ToString(), versionFileId.ToString());
		return await fileCache.SaveFileAsync(cachePath, fileStream);
	}
}
