using System;
using System.Collections.Generic;
using FiddlerBackendSDK.ConcurrencyHandling;

namespace FiddlerBackendSDK.ComposerCollections.Client;

public class ComposerCollection : ConcurrencyTokenAware
{
	public Guid Id { get; set; }

	public string Owner { get; set; }

	public Guid AccountId { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public int Version { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public IEnumerable<ComposerCollectionShareReceiver> SharedWith { get; set; }

	public IEnumerable<ComposerCollectionFolder> Folders { get; set; }

	public IEnumerable<ComposerCollectionRequest> Requests { get; set; }
}
