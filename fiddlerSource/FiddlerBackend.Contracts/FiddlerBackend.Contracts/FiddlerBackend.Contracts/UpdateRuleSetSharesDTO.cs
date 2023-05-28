using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class UpdateRuleSetSharesDTO : ConcurrencyTokenAwareDTO
{
	[FromRoute]
	public Guid RuleSetId { get; set; }

	[FromBody]
	public List<RuleSetShareDTO> Shares { get; set; } = new List<RuleSetShareDTO>();

}
