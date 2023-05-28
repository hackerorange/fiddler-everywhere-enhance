using System;
using System.Threading.Tasks;
using FiddlerBackendSDK.Notifications;

namespace FiddlerBackendSDK;

public interface IBackend
{
	T GetService<T>();

	Task<IObservable<NotificationMessage>> GetNotificationObservable(string uniqueClientId, string clientEmail, bool waitForSubscriptionConfirmation = false);

	string UpdateToken(string idToken);
}
