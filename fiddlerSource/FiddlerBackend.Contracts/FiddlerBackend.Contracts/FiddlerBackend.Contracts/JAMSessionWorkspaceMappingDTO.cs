using System;

namespace FiddlerBackend.Contracts;

public class JAMSessionWorkspaceMappingDTO
{
	public Guid SessionId { get; set; }

	public string Title { get; set; }

	public string Description { get; set; }

	public string SubmittedBy { get; set; }
}
