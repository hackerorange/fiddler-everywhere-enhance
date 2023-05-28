using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class GetJAMSessionDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string SharingToken { get; set; }

	[FromHeader(Name = "X-Upload-Token")]
	public string UploadToken { get; set; }
}
