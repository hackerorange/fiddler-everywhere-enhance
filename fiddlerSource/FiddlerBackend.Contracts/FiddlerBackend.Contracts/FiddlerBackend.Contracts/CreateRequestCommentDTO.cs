using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class CreateRequestCommentDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromRoute]
	public Guid RequestId { get; set; }

	[FromBody]
	public RequestCommentBodyDTO CommentBody { get; set; }
}
