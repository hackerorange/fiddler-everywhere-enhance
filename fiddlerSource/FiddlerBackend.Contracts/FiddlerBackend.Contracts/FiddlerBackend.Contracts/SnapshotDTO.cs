using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class SnapshotDTO : BaseSnapshotDTO
{
	public IList<SnapshotVersionFileDTO> Versions { get; set; } = new List<SnapshotVersionFileDTO>();


	public IList<SnapshotCloneDTO> Clones { get; set; } = new List<SnapshotCloneDTO>();

}
