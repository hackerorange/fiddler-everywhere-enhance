using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class CreateRuleSetDTO
{
	public Guid AccountId { get; set; }

	public string Name { get; set; }

	public IList<CreateAutoResponderRuleDTO> Rules { get; set; } = new List<CreateAutoResponderRuleDTO>();

}
