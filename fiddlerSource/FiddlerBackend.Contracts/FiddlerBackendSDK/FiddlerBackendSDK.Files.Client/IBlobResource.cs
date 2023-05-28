using System.Threading.Tasks;

namespace FiddlerBackendSDK.Files.Client;

public interface IBlobResource<in T>
{
	Task<string> GetContentAsStringAsync();
}
