using System.IO;

namespace FiddlerBackendSDK.AutoResponder.Client;

public class AutoResponderRule
{
	public string Match { get; set; }

	public string Action { get; set; }

	public string Comment { get; set; }

	public Stream Headers { get; set; }

	public Stream Body { get; set; }
}
