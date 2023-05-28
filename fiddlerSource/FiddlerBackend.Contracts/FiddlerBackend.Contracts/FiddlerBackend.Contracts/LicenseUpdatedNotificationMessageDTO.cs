using System;

namespace FiddlerBackend.Contracts;

public class LicenseUpdatedNotificationMessageDTO : NotificationMessageDTO
{
	public override FiddlerProduct Product => Account.Product;

	public AccountDTO Account { get; set; }

	public DateTime? EffectiveDate { get; set; }
}
