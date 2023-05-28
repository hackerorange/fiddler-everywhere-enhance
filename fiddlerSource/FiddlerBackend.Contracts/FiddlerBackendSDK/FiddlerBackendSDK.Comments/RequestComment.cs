using System;
using System.Collections.Generic;

namespace FiddlerBackendSDK.Comments;

public class RequestComment
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public string Owner { get; set; }

	public string Text { get; set; }

	public bool IsDeleted { get; set; }

	public Guid ParentCommentId { get; set; }

	public IEnumerable<RequestComment> Replies { get; set; } = new List<RequestComment>();

}
