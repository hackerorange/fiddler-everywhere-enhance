namespace FiddlerBackend.Contracts;

[ArtifactType("navigation")]
public class CreateJAMSessionNavigationDTO : CreateJAMSessionArtifactDTO
{
	public override string Type { get; set; } = "navigation";


	public string Url { get; set; }

	public string TransitionType { get; set; }

	public bool? IsHistoryStateUpdated { get; set; }

	public override void Validate()
	{
		base.Validate();
		if (string.IsNullOrEmpty(Url))
		{
			throw new ValidationException("Url field is required", "The Url field is required for creating a new JAM session navigation");
		}
	}
}
