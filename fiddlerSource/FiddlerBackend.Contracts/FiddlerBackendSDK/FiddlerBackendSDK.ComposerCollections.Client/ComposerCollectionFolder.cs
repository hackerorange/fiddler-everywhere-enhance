using System;
using FiddlerBackendSDK.ConcurrencyHandling;

namespace FiddlerBackendSDK.ComposerCollections.Client;

public class ComposerCollectionFolder : ConcurrencyTokenAware
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public Guid ParentId { get; set; }

	public Guid ComposerCollectionId { get; set; }

	public Guid AccountId { get; set; }

	public string Owner { get; set; }

	public string CreatedBy { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }
}
