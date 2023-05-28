using System;

namespace FiddlerBackendSDK.Files.Client;

public class RemoteFileMetadata
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public long Size { get; set; }

	public string ContentType { get; set; }

	public string ContentMD5 { get; set; }
}
