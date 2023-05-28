using System;

namespace FiddlerBackend.Contracts;

public class ComposerCollectionRequestParentDTO
{
	public Guid Id { get; set; }

	public bool IsRootCollection { get; set; }

	public string Name { get; set; }
}
