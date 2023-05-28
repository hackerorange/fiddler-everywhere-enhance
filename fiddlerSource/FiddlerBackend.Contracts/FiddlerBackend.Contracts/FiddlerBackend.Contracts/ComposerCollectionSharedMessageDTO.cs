using System.Collections.Generic;
using Newtonsoft.Json;

namespace FiddlerBackend.Contracts;

public class ComposerCollectionSharedMessageDTO : ComposerCollectionNotificationMessageDTO
{
	[JsonIgnore]
	public IEnumerable<ShareDTO> Shares { get; set; }

	public string ShareNote { get; set; }
}
