namespace FiddlerBackend.Contracts;

public class JAMSessionScrollDTO : JAMSessionArtifactWithHtmlElementDTO
{
	public string ScrollDirection { get; set; }

	public decimal ScrollPosition { get; set; }
}
