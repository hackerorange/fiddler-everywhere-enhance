using System.ComponentModel.DataAnnotations;

namespace FiddlerBackend.Contracts;

public class UpdateJamSessionWorkspaceDTO
{
	[Required]
	public string Title { get; set; }

	public string Description { get; set; }

	[Required]
	public string SubmittedBy { get; set; }
}
