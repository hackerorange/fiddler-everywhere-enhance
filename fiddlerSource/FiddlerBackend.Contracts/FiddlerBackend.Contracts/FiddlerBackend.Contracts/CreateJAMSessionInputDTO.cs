namespace FiddlerBackend.Contracts;

[ArtifactType("input")]
public class CreateJAMSessionInputDTO : CreateJAMSessionArtifactWithHtmlElementDTO
{
	public override string Type { get; set; } = "input";


	public override void Validate()
	{
		base.Validate();
		if (string.IsNullOrEmpty(base.Tag))
		{
			throw new ValidationException("Tag field is required", "The Tag field is required for creating a new JAM session input");
		}
	}
}
