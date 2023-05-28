using System.IO;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Files.Client;

public interface IMD5Calculator
{
	string Calculate(Stream stream);

	string Calculate(string filePath);

	string Calculate(byte[] bytes);

	Task DecryptAsync(Stream input, Stream output, string password, byte[] encryptionSalt, uint iterations);
}
