namespace FiddlerBackend.Contracts;

public class JAMWorkspaceSessionDTO
{
	public string Title { get; set; }

	public string Description { get; set; }

	public string SubmittedBy { get; set; }

	public JAMSessionDTO Session { get; set; }
}
