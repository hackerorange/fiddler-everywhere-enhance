using System;

namespace FiddlerBackend.Contracts;

public class ComposerCollectionPublicSharingTokenResponseDTO
{
	public Guid ComposerCollectionId { get; set; }

	public string PublicSharingToken { get; set; }
}
