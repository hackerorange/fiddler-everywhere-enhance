using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class InitiateMultipartUploadResponseDTO
{
	public Guid FileId { get; set; }

	public long ChunkSize { get; set; }

	public IList<string> UploadUrls { get; set; }
}
