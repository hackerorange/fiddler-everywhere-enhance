using Newtonsoft.Json;

namespace FiddlerBackendSDK.DTO;

public class CouponDTO
{
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("duration")]
	public string Duration { get; set; }

	[JsonProperty("duration_in_months")]
	public int? DurationInMonths { get; set; }

	[JsonProperty("percent_off")]
	public int? PercentOff { get; set; }

	[JsonProperty("amount_off")]
	public int? AmountOff { get; set; }

	[JsonProperty("applies_to")]
	public string AppliesTo { get; set; }

	[JsonProperty("currency")]
	public string Currency { get; set; }

	[JsonProperty("max_redemptions")]
	public int? MaxRedemptions { get; set; }

	[JsonProperty("times_redeemed")]
	public int TimesRedeemed { get; set; }

	[JsonProperty("valid")]
	public bool IsValid { get; set; }
}
