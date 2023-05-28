using System;

namespace FiddlerBackendSDK.Snapshot.Client;

public class LocalSnapshotRequestComment
{
	public Guid Id { get; set; }

	public Guid RequestId { get; set; }

	public Guid? ParentCommentId { get; set; }

	public string Text { get; set; }
}
