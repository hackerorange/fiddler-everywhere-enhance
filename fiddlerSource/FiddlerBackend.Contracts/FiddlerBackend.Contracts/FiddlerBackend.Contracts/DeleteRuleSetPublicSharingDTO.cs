using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class DeleteRuleSetPublicSharingDTO
{
	[FromRoute]
	public Guid RuleSetId { get; set; }

	[FromHeader(Name = "X-Sharing-Token")]
	public string PublicSharingToken { get; set; }
}
