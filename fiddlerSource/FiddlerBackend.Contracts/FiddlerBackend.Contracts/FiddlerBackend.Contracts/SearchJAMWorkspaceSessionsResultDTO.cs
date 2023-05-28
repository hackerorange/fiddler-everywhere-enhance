using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class SearchJAMWorkspaceSessionsResultDTO
{
	public long TotalCount { get; set; }

	public IList<JAMWorkspaceSessionDTO> Sessions { get; set; } = new List<JAMWorkspaceSessionDTO>();

}
