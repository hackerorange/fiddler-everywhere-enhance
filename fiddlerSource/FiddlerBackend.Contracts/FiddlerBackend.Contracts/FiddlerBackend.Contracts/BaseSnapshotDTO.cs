using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class BaseSnapshotDTO : ConcurrencyTokenAwareDTO
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public Guid AccountId { get; set; }

	public string Owner { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public bool IsDeleted { get; set; }

	public bool IsPasswordProtected { get; set; }

	public FileDTO File { get; set; }

	public IList<ShareDTO> SharedWith { get; set; } = new List<ShareDTO>();

}
