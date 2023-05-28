namespace FiddlerBackend.Contracts;

[ArtifactType("websocket")]
public class CreateJAMSessionWebsocketDTO : CreateJAMSessionArtifactDTO
{
	public override string Type { get; set; } = "websocket";


	public string Url { get; set; }

	public bool? IsClosed { get; set; }

	public override void Validate()
	{
		base.Validate();
		if (!base.StartOffset.HasValue)
		{
			throw new ValidationException("StartOffset field is required", "The StartOffset field is required for creating a new JAM session request");
		}
		if (!base.EndOffset.HasValue)
		{
			throw new ValidationException("EndOffset field is required", "The EndOffset field is required for creating a new JAM session request");
		}
		if (string.IsNullOrEmpty(Url))
		{
			throw new ValidationException("Url field is required", "The Url field is required for creating a new JAM session request");
		}
	}
}
