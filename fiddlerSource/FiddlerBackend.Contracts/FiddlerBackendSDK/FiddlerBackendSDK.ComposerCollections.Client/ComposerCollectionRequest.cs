using System;
using FiddlerBackendSDK.ConcurrencyHandling;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.ComposerCollections.Client;

public class ComposerCollectionRequest : ConcurrencyTokenAware
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Url { get; set; }

	public string Parameters { get; set; }

	public string HttpMethod { get; set; }

	public string HttpVersion { get; set; }

	public Guid ParentId { get; set; }

	public Guid ComposerCollectionId { get; set; }

	public Guid AccountId { get; set; }

	public string Owner { get; set; }

	public string CreatedBy { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public IBlobResource<string> RequestHeadersFile { get; set; }

	public IBlobResource<string> RequestBodyFile { get; set; }
}
