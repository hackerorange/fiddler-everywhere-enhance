using System;

namespace FiddlerBackend.Contracts;

public class FileDTO
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public long Size { get; set; }

	public string ContentType { get; set; }

	public string ContentMD5 { get; set; }

	public bool HasPassword { get; set; }
}
