using System.Collections.Generic;

namespace FiddlerBackendSDK.Snapshot.Client;

public class SnapshotMetadata : BaseSnapshotMetadata
{
	public IEnumerable<SnapshotVersionFileMetadata> SnapshotFileVersions { get; set; }
}
