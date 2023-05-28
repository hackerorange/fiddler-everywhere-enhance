using FiddlerBackend.Contracts;
using Newtonsoft.Json.Linq;

namespace FiddlerBackendSDK.Notifications.Serialization;

public interface INotificationMessageDeserializer
{
	NotificationMessageDTO Deserialize(string payload);

	NotificationMessageDTO Deserialize(JObject payload);
}
