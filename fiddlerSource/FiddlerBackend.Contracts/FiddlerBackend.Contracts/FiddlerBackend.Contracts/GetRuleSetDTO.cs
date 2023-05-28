using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class GetRuleSetDTO
{
	[FromRoute]
	public Guid Id { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string PublicSharingToken { get; set; }
}
