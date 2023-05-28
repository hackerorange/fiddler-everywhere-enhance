namespace FiddlerBackend.Contracts;

[ArtifactType("key")]
public class CreateJAMSessionKeyDTO : CreateJAMSessionArtifactDTO
{
	public override string Type { get; set; } = "key";


	public string Key { get; set; }

	public override void Validate()
	{
		base.Validate();
		if (string.IsNullOrEmpty(Key))
		{
			throw new ValidationException("Key field is required", "The Key field is required for creating a new JAM session key");
		}
	}
}
