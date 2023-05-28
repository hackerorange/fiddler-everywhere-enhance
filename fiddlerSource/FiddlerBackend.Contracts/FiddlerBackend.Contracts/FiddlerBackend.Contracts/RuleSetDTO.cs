using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class RuleSetDTO : ConcurrencyTokenAwareDTO
{
	public Guid Id { get; set; }

	public DateTime CreatedAt { get; set; }

	public DateTime ModifiedAt { get; set; }

	public string Owner { get; set; }

	public Guid AccountId { get; set; }

	public string Name { get; set; }

	public IList<AutoResponderRuleDTO> Rules { get; set; } = new List<AutoResponderRuleDTO>();


	public IList<ShareDTO> SharedWith { get; set; } = new List<ShareDTO>();

}
