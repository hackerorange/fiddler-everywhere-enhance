using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class PatchJAMSessionWorkspaceDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromRoute]
	public Guid SessionId { get; set; }

	[FromBody]
	public UpdateJamSessionWorkspaceDTO UpdateJamSessionWorkspace { get; set; }
}
