namespace FiddlerBackendSDK.Subscription;

public class SubscriptionClient : ISubscriptionClient
{
    public Task StartTrialAsync(string machineId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsTrialAvailableAsync(string machineId)
    {
        return Task.FromResult(true);
    }

    public Task<string> CreateToken(string idToken, string refreshToken)
    {
        throw new NotImplementedException();
    }
}