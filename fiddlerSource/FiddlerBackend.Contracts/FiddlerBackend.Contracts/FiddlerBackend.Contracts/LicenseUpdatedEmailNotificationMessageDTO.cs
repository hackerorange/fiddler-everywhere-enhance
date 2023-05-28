using System;

namespace FiddlerBackend.Contracts;

public class LicenseUpdatedEmailNotificationMessageDTO : LicenseUpdatedNotificationMessageDTO
{
	public string ProductDisplayName => $"Fiddler {Product}";

	public string ProductUppercaseDisplayName => ProductDisplayName.ToUpper();

	public string LicenseDisplayName => base.Account.LicensePlanName switch
	{
		"trial" => "Trial", 
		"pro" => "Pro", 
		"enterprise" => "Enterprise", 
		"jam_trial" => "Trial", 
		"jam_starter" => "Starter", 
		"jam_growth" => "Growth", 
		"jam_business" => "Business", 
		_ => string.Empty, 
	};

	public string ProductWithLicenseDisplayName => ProductDisplayName + " " + LicenseDisplayName;

	public string ProductMainAction
	{
		get
		{
			if (Product != FiddlerProduct.Everywhere)
			{
				return "troubleshooting";
			}
			return "debugging";
		}
	}

	public string ProductUtmInputCampaign => Product.ToString().ToLower();

	public string StartUsingFiddlerProduct => Environment.GetEnvironmentVariable($"StartUsingFiddler{Product}");

	public string ManageSubscription => Environment.GetEnvironmentVariable("ManageSubscription");

	public string EffectiveDateDisplay => base.EffectiveDate?.ToString("dddd, MMMM dd, yyyy");
}
