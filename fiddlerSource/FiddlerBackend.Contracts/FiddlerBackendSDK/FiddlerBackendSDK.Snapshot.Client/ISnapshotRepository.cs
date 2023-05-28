using System;
using System.IO;
using System.Threading.Tasks;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.Snapshot.Client;

public interface ISnapshotRepository
{
	Task<string> GetFileLocationAsync(SnapshotMetadata snapshot, RemoteFileMetadata snapshotRemoteFile);

	Task<string> GetVersionFileLocationAsync(SnapshotMetadata snapshot, SnapshotVersionFileMetadata versionFile, string password);

	Task<string> GetTempVersionFileDeltaAsync(SnapshotMetadata snapshot, string versionFile, string password);

	string AddFileToCache(Guid snapshotId, Guid fileId, string filePath, bool deleteOriginal = false);

	Task<string> AddFileToCache(Guid snapshotId, Guid fileId, Stream fileStream);
}
