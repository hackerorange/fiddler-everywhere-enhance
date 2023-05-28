using PubnubApi;

namespace FiddlerBackendSDK.Notifications.Pubnub;

public interface IPubnubClient
{
	void AddListener(SubscribeCallback subscribeCallback);

	void RemoveListener(SubscribeCallback subscribeCallback);

	void Subscribe(string channelName);

	void Unsubscribe(string channelName);
}
