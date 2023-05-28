using System;
using System.Collections.Generic;
using FiddlerBackendSDK.ConcurrencyHandling;
using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.Snapshot.Client;

public class BaseSnapshotMetadata : ConcurrencyTokenAware
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public Guid AccountId { get; set; }

	public string Owner { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public bool IsDeleted { get; set; }

	public bool IsPasswordProtected { get; set; }

	public RemoteFileMetadata SnapshotFile { get; set; }

	public IEnumerable<SnapshotShareReceiver> SharedWith { get; set; }
}
