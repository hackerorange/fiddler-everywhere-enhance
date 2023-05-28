namespace FiddlerBackend.Contracts;

public class RuleSetNotificationMessageDTO : NotificationMessageDTO
{
	public override FiddlerProduct Product => FiddlerProduct.Everywhere;

	public RuleSetDTO RuleSet { get; set; }
}
