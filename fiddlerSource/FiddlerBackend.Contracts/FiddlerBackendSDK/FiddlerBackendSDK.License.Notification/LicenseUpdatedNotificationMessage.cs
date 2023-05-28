using System;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Notifications;

namespace FiddlerBackendSDK.License.Notification;

public class LicenseUpdatedNotificationMessage : NotificationMessage
{
	public AccountDTO Account { get; set; }

	public DateTime? EffectiveDate { get; set; }
}
