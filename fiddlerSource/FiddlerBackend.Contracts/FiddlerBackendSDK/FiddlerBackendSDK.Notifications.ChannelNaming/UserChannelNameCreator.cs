using System.Security.Cryptography;
using System.Text;

namespace FiddlerBackendSDK.Notifications.ChannelNaming;

internal class UserChannelNameCreator : IUserChannelNameCreator
{
	private readonly HashAlgorithm hashAlgorithm;

	public UserChannelNameCreator(HashAlgorithm hashAlgorithm)
	{
		this.hashAlgorithm = hashAlgorithm;
	}

	public string CreateUniqueChannelName(string userEmail)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(userEmail.ToLower());
		byte[] array = hashAlgorithm.ComputeHash(bytes);
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			stringBuilder.Append(b.ToString("x2"));
		}
		return stringBuilder.ToString();
	}
}
