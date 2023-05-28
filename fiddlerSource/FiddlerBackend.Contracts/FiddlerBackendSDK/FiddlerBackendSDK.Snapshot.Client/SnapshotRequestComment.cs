using System;
using FiddlerBackendSDK.Comments;

namespace FiddlerBackendSDK.Snapshot.Client;

public class SnapshotRequestComment : RequestComment
{
	public Guid RequestId { get; set; }
}
