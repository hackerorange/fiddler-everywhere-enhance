using FiddlerBackend.Contracts.DTO.Snapshots;

namespace FiddlerBackend.Contracts;

public class SnapshotCommentNotificationMessageDTO : NotificationMessageDTO
{
	public override FiddlerProduct Product => FiddlerProduct.Everywhere;

	public SnapshotRequestCommentDTO SnapshotRequestComment { get; set; }
}
