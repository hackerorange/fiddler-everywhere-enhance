using System.Threading.Tasks;

namespace FiddlerBackendSDK.Subscription;

public interface ISubscriptionClient
{
	Task StartTrialAsync(string machineId);

	Task<bool> IsTrialAvailableAsync(string machineId);

	Task<string> CreateToken(string idToken, string refreshToken);
}
