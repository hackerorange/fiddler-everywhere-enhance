using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class ComposerCollectionDTO : ConcurrencyTokenAwareDTO
{
	public Guid Id { get; set; }

	public string Owner { get; set; }

	public Guid AccountId { get; set; }

	public int Version { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public ICollection<ComposerCollectionFolderDTO> Folders { get; set; } = new List<ComposerCollectionFolderDTO>();


	public ICollection<ComposerCollectionRequestDTO> Requests { get; set; } = new List<ComposerCollectionRequestDTO>();


	public IList<ShareDTO> SharedWith { get; set; } = new List<ShareDTO>();


	public IList<ComposerCollectionCloneDTO> Clones { get; set; } = new List<ComposerCollectionCloneDTO>();

}
