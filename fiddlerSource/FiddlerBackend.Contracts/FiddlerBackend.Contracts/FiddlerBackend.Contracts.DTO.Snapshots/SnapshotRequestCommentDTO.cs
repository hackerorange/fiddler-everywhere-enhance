using System;

namespace FiddlerBackend.Contracts.DTO.Snapshots;

public class SnapshotRequestCommentDTO : RequestCommentDTO
{
	public Guid RequestId { get; set; }
}
