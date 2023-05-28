using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class CreateComposerCollectionDTO
{
	public Guid Id { get; set; }

	public Guid AccountId { get; set; }

	public int Version { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DateTime CreatedAt { get; set; }

	public ICollection<ComposerCollectionFolderDTO> Folders { get; set; } = new List<ComposerCollectionFolderDTO>();


	public ICollection<CreateComposerCollectionRequestDTO> Requests { get; set; } = new List<CreateComposerCollectionRequestDTO>();

}
