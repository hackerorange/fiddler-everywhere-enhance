using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class PatchJAMWorkspaceDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromBody]
	public UpdateJamWorkspaceDTO UpdateJamWorkspace { get; set; }
}
