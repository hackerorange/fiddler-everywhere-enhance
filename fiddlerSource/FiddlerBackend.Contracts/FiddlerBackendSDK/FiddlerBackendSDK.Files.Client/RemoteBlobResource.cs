using System;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Files.Client;

public class RemoteBlobResource<T> : RemoteFileMetadata, IBlobResource<T>
{
	private readonly Guid? remoteFileId;

	private readonly Func<Guid?, Task<string>> downloader;

	internal RemoteBlobResource(Guid? remoteFileId, Func<Guid?, Task<string>> downloader)
	{
		this.remoteFileId = remoteFileId;
		this.downloader = downloader;
	}

	public async Task<string> GetContentAsStringAsync()
	{
		return await downloader(remoteFileId);
	}
}
