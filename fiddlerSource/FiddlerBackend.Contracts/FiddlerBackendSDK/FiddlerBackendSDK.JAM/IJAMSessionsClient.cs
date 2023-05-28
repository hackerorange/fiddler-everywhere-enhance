using System;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.JAM;

public interface IJAMSessionsClient
{
	Task<JAMWorkspaceSessionDTO> GetWorkspaceSessionAsync(Guid workspaceId, Guid sessionId);

	Task<JAMSessionDTO> GetAsync(Guid sessionId, string sharingToken);

	Task DownloadJAMSessionAsync(JAMSessionDTO jamSession, string filePath, string sharingToken = null, string password = null);
}
