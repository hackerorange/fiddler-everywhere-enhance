using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class SearchJAMSessionArtifactsResultDTO
{
	public long TotalCount { get; set; }

	public IList<object> Artifacts { get; set; } = new List<object>();

}
