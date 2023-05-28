using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Notifications;

namespace FiddlerBackendSDK.AutoResponder.Notification;

public class RuleSetNotificationMessage : NotificationMessage
{
	public RuleSetDTO RuleSet { get; set; }
}
