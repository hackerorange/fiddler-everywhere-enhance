namespace FiddlerBackend.Contracts;

public abstract class JAMSessionArtifactWithHtmlElementDTO : JAMSessionArtifactDTO
{
	public string Tag { get; set; }

	public string ElementClass { get; set; }

	public string ElementId { get; set; }
}
