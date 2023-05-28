using System.Threading.Tasks;

namespace FiddlerBackendSDK.Files.Client;

public class LocalBlobResource<T> : IBlobResource<T>
{
	private readonly T content;

	public LocalBlobResource(T content)
	{
		this.content = content;
	}

	public async Task<string> GetContentAsStringAsync()
	{
		return await Task.FromResult(content as string);
	}
}
