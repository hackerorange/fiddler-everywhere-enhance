using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class GetUpdatesDTO
{
	[FromRoute]
	public string Client { get; set; }

	[FromRoute]
	public string Version { get; set; }

	[FromRoute]
	public string Platform { get; set; }

	[FromRoute]
	public string Key { get; set; }
}
