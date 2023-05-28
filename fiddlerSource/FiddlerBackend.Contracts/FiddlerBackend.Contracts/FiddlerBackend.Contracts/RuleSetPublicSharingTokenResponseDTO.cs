using System;

namespace FiddlerBackend.Contracts;

public class RuleSetPublicSharingTokenResponseDTO
{
	public Guid RuleSetId { get; set; }

	public string PublicSharingToken { get; set; }
}
