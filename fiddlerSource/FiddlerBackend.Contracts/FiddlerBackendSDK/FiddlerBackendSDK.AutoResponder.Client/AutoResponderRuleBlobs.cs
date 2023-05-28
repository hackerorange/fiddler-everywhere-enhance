using System;

namespace FiddlerBackendSDK.AutoResponder.Client;

public class AutoResponderRuleBlobs
{
	public Guid Id { get; set; }

	public string HeadersFile { get; set; }

	public string BodyFile { get; set; }
}
