namespace FiddlerBackend.Contracts;

[ArtifactType("tab")]
public class CreateJAMSessionTabDTO : CreateJAMSessionArtifactDTO
{
	public override string Type { get; set; } = "tab";


	public string Url { get; set; }

	public string Title { get; set; }

	public bool? IsClosed { get; set; }

	public long? OpenerTabId { get; set; }

	public override void Validate()
	{
		base.Validate();
		if (string.IsNullOrEmpty(Url))
		{
			throw new ValidationException("Url field is required", "The Url field is required for creating a new JAM session tab");
		}
	}
}
