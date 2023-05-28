using System;
using System.Collections.Generic;
using FiddlerBackend.Contracts.DTO.Snapshots;

namespace FiddlerBackend.Contracts;

public class CreateSnapshotDTO : ConcurrencyTokenAwareDTO
{
	public Guid? Id { get; set; }

	public Guid AccountId { get; set; }

	public Guid FileId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public bool IsPasswordProtected { get; set; }

	public IEnumerable<SnapshotRequestCommentDTO> Comments { get; set; } = new List<SnapshotRequestCommentDTO>();

}
