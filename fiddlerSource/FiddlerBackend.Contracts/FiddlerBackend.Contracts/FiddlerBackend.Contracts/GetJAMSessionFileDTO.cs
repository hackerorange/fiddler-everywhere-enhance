using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class GetJAMSessionFileDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromUrlEncodedHeader(Name = "X-Auth-Pass")]
	public string Password { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string SharingToken { get; set; }

	[FromHeader(Name = "X-Public-Sharing-Token")]
	public string PublicSharingToken { get; set; }
}
