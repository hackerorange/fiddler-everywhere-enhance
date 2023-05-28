using Microsoft.AspNetCore.Mvc;

namespace FiddlerBackend;

public class StartTrialDTO
{
	[FromBody]
	public string MachineId { get; set; }
}
