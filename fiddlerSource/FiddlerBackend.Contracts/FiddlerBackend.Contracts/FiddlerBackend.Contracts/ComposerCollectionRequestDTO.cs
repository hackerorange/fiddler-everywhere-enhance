using System;

namespace FiddlerBackend.Contracts;

public class ComposerCollectionRequestDTO : ConcurrencyTokenAwareDTO
{
	public Guid Id { get; set; }

	public Guid AccountId { get; set; }

	public string Owner { get; set; }

	public string CreatedBy { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Url { get; set; }

	public string Parameters { get; set; }

	public string HttpMethod { get; set; }

	public string HttpVersion { get; set; }

	public Guid ParentId { get; set; }

	public Guid ComposerCollectionId { get; set; }

	public FileDTO RequestHeadersFile { get; set; }

	public FileDTO RequestBodyFile { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }
}
