using System.Collections.Generic;
using Newtonsoft.Json;

namespace FiddlerBackend.Contracts;

public class RuleSetSharedMessageDTO : RuleSetNotificationMessageDTO, IEntitySharedMessageDTO
{
	[JsonIgnore]
	public IEnumerable<ShareDTO> Shares { get; set; }

	public string ShareNote { get; set; }
}
