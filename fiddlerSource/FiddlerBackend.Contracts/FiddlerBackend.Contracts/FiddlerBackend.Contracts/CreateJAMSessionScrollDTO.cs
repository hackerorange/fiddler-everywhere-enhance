namespace FiddlerBackend.Contracts;

[ArtifactType("scroll")]
public class CreateJAMSessionScrollDTO : CreateJAMSessionArtifactWithHtmlElementDTO
{
	public override string Type { get; set; } = "scroll";


	public string ScrollDirection { get; set; }

	public decimal ScrollPosition { get; set; }
}
