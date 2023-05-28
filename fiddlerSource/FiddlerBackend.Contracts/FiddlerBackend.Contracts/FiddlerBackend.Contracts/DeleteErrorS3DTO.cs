namespace FiddlerBackend.Contracts;

public class DeleteErrorS3DTO
{
	public string Key { get; set; }

	public string VersionId { get; set; }

	public string Code { get; set; }

	public string Message { get; set; }
}
