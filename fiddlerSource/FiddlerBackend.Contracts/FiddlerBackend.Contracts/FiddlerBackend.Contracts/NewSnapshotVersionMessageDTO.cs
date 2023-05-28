namespace FiddlerBackend.Contracts;

public class NewSnapshotVersionMessageDTO : SnapshotNotificationMessageDTO
{
	public FileDTO Version { get; set; }
}
