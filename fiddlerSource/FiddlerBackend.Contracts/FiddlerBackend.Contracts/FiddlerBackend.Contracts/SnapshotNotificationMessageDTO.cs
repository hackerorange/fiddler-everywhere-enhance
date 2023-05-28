namespace FiddlerBackend.Contracts;

public class SnapshotNotificationMessageDTO : NotificationMessageDTO
{
	public override FiddlerProduct Product => FiddlerProduct.Everywhere;

	public BaseSnapshotDTO Snapshot { get; set; }
}
