using System;

namespace FiddlerBackend.Contracts.DTO.Snapshots;

public class LocalSnapshotRequestCommentDTO
{
	public Guid Id { get; set; }

	public Guid RequestId { get; set; }

	public Guid? ParentCommentId { get; set; }

	public string Text { get; set; }
}
