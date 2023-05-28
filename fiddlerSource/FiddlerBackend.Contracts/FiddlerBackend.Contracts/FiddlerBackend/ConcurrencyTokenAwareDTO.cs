using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend;

public class ConcurrencyTokenAwareDTO
{
	[FromHeader(Name = "If-Match")]
	public string ConcurrencyToken { get; set; }
}
