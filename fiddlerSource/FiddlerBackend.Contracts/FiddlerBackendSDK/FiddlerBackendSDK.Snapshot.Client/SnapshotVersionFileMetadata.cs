using FiddlerBackendSDK.Files.Client;

namespace FiddlerBackendSDK.Snapshot.Client;

public class SnapshotVersionFileMetadata : RemoteFileMetadata
{
	public bool IsDelta { get; set; }

	public bool IsPasswordProtected { get; set; }

	public string CreatedBy { get; set; }
}
