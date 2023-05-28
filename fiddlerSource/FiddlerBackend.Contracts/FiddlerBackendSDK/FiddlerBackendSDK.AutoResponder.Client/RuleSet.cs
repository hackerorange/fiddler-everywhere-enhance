using System.Collections.Generic;

namespace FiddlerBackendSDK.AutoResponder.Client;

public class RuleSet
{
	public string Name { get; set; }

	public ICollection<AutoResponderRule> Rules { get; set; } = new List<AutoResponderRule>();

}
