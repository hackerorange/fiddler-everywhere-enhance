using System.ComponentModel.DataAnnotations;

namespace FiddlerBackend.Contracts;

public class UpdateJamWorkspaceDTO
{
	[Required]
	public string Name { get; set; }
}
