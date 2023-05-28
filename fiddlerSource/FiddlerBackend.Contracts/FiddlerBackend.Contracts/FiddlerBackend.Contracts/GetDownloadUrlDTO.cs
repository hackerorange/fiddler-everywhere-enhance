using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class GetDownloadUrlDTO
{
	[FromRoute]
	public Guid FileId { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string PublicSharingToken { get; set; }

	[FromUrlEncodedHeader(Name = "X-Auth-Pass")]
	public string Password { get; set; }
}
