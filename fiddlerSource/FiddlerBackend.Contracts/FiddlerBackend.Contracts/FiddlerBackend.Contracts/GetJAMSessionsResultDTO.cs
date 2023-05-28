using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class GetJAMSessionsResultDTO
{
	public long TotalCount { get; set; }

	public IList<JAMSessionWithPublicUrlDTO> Sessions { get; set; } = new List<JAMSessionWithPublicUrlDTO>();

}
