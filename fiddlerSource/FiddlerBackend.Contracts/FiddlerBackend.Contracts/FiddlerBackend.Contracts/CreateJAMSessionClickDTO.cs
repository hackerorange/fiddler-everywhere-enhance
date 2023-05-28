namespace FiddlerBackend.Contracts;

[ArtifactType("click")]
public class CreateJAMSessionClickDTO : CreateJAMSessionArtifactWithHtmlElementDTO
{
	public override string Type { get; set; } = "click";


	public string Text { get; set; }

	public string Href { get; set; }

	public bool? IsDoubleClick { get; set; }

	public override void Validate()
	{
		base.Validate();
		if (string.IsNullOrEmpty(base.Tag))
		{
			throw new ValidationException("Tag field is required", "The Tag field is required for creating a new JAM session click");
		}
		if (!IsDoubleClick.HasValue)
		{
			throw new ValidationException("IsDoubleClick field is required", "The IsDoubleClick field is required for creating a new JAM session click");
		}
	}
}
