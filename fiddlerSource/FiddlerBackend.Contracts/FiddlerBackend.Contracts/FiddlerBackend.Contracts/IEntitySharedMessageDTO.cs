using System.Collections.Generic;
using Newtonsoft.Json;

namespace FiddlerBackend.Contracts;

public interface IEntitySharedMessageDTO
{
	[JsonIgnore]
	IEnumerable<ShareDTO> Shares { get; set; }

	string ShareNote { get; set; }
}
