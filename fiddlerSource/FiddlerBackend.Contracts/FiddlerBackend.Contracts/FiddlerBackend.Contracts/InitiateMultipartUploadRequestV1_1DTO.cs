using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class InitiateMultipartUploadRequestV1_1DTO : InitiateMultipartUploadRequestDTO
{
	[Required]
	[FromHeader(Name = "X-Upload-Content-MD5")]
	public string ContentMD5 { get; set; }

	[Required]
	[FromHeader(Name = "X-Upload-Chunk-Checksums")]
	public string ChunkChecksums { get; set; }

	[Required]
	[FromHeader(Name = "X-Upload-Chunk-Size")]
	public long ChunkSize { get; set; }
}
