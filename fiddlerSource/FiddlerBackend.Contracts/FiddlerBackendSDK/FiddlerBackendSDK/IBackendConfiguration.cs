using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FiddlerBackendSDK;

public interface IBackendConfiguration
{
	string ApiEndpoint { get; }

	string IdentityEndpoint { get; }

	string BearerToken { get; set; }

	IWebProxy Proxy { get; }

	string CacheFolder { get; }

	Func<string, Task<string>> ReissueToken { get; }

	ILoggerProvider LoggerProvider { get; }

	bool DisableConcurrency { get; }

	string VersionHeaderName { get; }

	string DefaultApiVersion { get; }

	bool ByteRangeDownloadEnabled { get; }

	int RetriesCount { get; set; }

	Func<TimeSpan> RetriesIntervalProvider { get; set; }
}
