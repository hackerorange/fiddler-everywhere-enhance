using System;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class DeleteRuleSetDTO : ConcurrencyTokenAwareDTO
{
	[FromRoute]
	public Guid RuleSetId { get; set; }
}
