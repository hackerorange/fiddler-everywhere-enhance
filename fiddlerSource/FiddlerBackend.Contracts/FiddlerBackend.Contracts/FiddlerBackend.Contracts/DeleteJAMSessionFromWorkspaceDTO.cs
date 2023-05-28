using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class DeleteJAMSessionFromWorkspaceDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromRoute]
	public Guid SessionId { get; set; }
}
