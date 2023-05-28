using System.Collections.Generic;
using Newtonsoft.Json;

namespace FiddlerBackend.Contracts;

public class SnapshotSharedMessageDTO : SnapshotNotificationMessageDTO, IEntitySharedMessageDTO
{
	[JsonIgnore]
	public IEnumerable<ShareDTO> Shares { get; set; }

	public string ShareNote { get; set; }
}
