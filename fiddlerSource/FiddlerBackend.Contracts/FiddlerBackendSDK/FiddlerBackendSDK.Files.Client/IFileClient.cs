using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.Files.Client;

public interface IFileClient
{
	long ChunkSize { get; }

	Task<string> GetFileUrlAsync(Guid fileId);

	Task<InitiateMultipartUploadResponseDTO> InitiateMultipartFileUploadAsync(Guid accountId, Dictionary<string, string> headers);

	Task<IEnumerable<PartETagDTO>> UploadToS3Async(Stream input, IList<string> uploadUrls);

	Task CompleteFileUploadAsync(Guid accountId, Guid fileId, IEnumerable<PartETagDTO> etags);
}
