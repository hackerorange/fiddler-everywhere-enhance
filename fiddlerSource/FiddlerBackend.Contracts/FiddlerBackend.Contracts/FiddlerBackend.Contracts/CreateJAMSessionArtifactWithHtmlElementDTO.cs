namespace FiddlerBackend.Contracts;

public abstract class CreateJAMSessionArtifactWithHtmlElementDTO : CreateJAMSessionArtifactDTO
{
	public string Tag { get; set; }

	public string ElementClass { get; set; }

	public string ElementId { get; set; }

	public CreateJAMSessionArtifactWithHtmlElementDTO()
	{
	}
}
