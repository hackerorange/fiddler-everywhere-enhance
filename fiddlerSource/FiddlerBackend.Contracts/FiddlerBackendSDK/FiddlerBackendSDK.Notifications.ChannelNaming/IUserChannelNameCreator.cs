using System.Diagnostics.CodeAnalysis;

namespace FiddlerBackendSDK.Notifications.ChannelNaming;

public interface IUserChannelNameCreator
{
	string CreateUniqueChannelName([NotNull] string userEmail);
}
