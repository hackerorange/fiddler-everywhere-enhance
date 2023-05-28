using System;

namespace FiddlerBackend.Contracts;

public class JAMWorkspaceDTO
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Owner { get; set; }
}
