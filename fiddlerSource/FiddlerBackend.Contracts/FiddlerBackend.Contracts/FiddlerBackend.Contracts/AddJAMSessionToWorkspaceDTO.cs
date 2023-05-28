using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class AddJAMSessionToWorkspaceDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string SharingToken { get; set; }

	[FromBody]
	public JAMSessionWorkspaceMappingDTO Mapping { get; set; }
}
