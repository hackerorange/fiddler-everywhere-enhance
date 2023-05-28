namespace FiddlerBackend.Contracts;

public class PushNotificationsConfigurationResponseDTO
{
	public string PubnubSubscribeKey { get; set; }

	public PushNotificationsConfigurationResponseDTO(string pubnubSubscribeKey)
	{
		PubnubSubscribeKey = pubnubSubscribeKey;
	}
}
