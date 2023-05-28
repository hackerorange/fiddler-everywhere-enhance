namespace FiddlerBackend.Contracts;

public class GetJAMSessionFileResultDTO
{
	public string DownloadUrl { get; set; }

	public FileDTO Metadata { get; set; }
}
