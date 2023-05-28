using System.Threading.Tasks;

namespace FiddlerBackendSDK.Notifications.Pubnub;

public interface IPubnubClientFactory
{
	Task<IPubnubClient> CreateAsync(string uniqueClientId, bool waitForSubscriptionConfirmation = false);
}
