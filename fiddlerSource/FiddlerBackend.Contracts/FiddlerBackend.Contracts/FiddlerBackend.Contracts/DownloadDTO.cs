using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend.Contracts;

public class DownloadDTO
{
	[FromRoute]
	public string Platform { get; set; }

	[FromRoute]
	public string Channel { get; set; }
}
