namespace FiddlerBackend.Contracts;

public class JAMSessionClickDTO : JAMSessionArtifactWithHtmlElementDTO
{
	public string Text { get; set; }

	public string Href { get; set; }

	public bool? IsDoubleClick { get; set; }
}
