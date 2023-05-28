using Newtonsoft.Json;

namespace FiddlerBackendSDK.Subscription;

public class CreateTokenDTO
{
	[JsonProperty(PropertyName = "idToken")]
	public string IdToken { get; set; }

	[JsonProperty(PropertyName = "refreshToken")]
	public string RefreshToken { get; set; }
}
