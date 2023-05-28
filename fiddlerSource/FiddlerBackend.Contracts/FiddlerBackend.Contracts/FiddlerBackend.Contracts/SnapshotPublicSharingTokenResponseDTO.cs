using System;

namespace FiddlerBackend.Contracts;

public class SnapshotPublicSharingTokenResponseDTO
{
	public Guid SnapshotId { get; set; }

	public string PublicSharingToken { get; set; }
}
