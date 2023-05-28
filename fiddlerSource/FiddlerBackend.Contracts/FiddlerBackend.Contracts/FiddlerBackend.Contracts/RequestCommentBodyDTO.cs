using System;

namespace FiddlerBackend.Contracts;

public class RequestCommentBodyDTO
{
	public Guid? ParentCommentId { get; set; }

	public string Text { get; set; }
}
