namespace FiddlerBackend.Contracts;

public class ComposerCollectionNotificationMessageDTO : NotificationMessageDTO
{
	public override FiddlerProduct Product => FiddlerProduct.Everywhere;

	public ComposerCollectionDTO ComposerCollection { get; set; }
}
