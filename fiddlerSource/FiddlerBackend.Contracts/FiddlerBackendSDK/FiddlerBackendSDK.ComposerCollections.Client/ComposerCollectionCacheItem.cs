using System;
using FiddlerBackendSDK.Core;

namespace FiddlerBackendSDK.ComposerCollections.Client;

public class ComposerCollectionCacheItem : IEntity
{
	public Guid Id { get; set; }

	public Guid AccountId { get; set; }

	public string Owner { get; set; }
}
