using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class GetJAMSessionFromWorkspaceDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromRoute]
	public Guid SessionId { get; set; }

	[FromHeader(Name = "X-Public-Sharing-Token")]
	public string PublicSharingToken { get; set; }

	[FromHeader(Name = "X-Upload-Token")]
	public string UploadToken { get; set; }
}
