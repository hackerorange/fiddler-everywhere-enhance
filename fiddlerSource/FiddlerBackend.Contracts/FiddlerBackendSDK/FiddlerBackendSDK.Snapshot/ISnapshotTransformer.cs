using System.IO;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Snapshot;

public interface ISnapshotTransformer
{
	Task<Stream> CreateDeltaAsync(string mainFilePath, string versionFilePath, string password);

	Task<Stream> ApplyDeltaAsync(string mainFilePath, string versionFilePath, string password);
}
