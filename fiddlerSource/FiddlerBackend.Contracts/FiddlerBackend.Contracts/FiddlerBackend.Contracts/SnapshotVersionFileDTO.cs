namespace FiddlerBackend.Contracts;

public class SnapshotVersionFileDTO : FileDTO
{
	public bool IsDelta { get; set; }

	public bool IsPasswordProtected { get; set; }

	public string CreatedBy { get; set; }
}
