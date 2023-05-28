using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class UpdateRequestCommentDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromBody]
	public RequestCommentBodyDTO CommentBody { get; set; }
}
