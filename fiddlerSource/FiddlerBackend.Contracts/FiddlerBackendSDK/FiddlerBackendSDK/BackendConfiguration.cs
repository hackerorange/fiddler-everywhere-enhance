using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FiddlerBackendSDK;

public class BackendConfiguration : IBackendConfiguration
{
	private string apiEndpoint;

	private string identityEndpoint;

	private string bearerToken;

	private IWebProxy proxy;

	private string cacheFolder;

	public string VersionHeaderName => "Api-Version";

	public string DefaultApiVersion => "1.0";

	public Func<string, Task<string>> ReissueToken { get; private set; }

	public ILoggerProvider LoggerProvider { get; private set; }

	public bool DisableConcurrency { get; private set; }

	public bool ByteRangeDownloadEnabled { get; private set; }

	public string ApiEndpoint
	{
		get
		{
			return apiEndpoint;
		}
		private set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException("Api endpoint shouldn't be blank or null!", "ApiEndpoint");
			}
			apiEndpoint = ((value.Last() == '/') ? value : (value + "/"));
		}
	}

	public string IdentityEndpoint
	{
		get
		{
			return identityEndpoint;
		}
		private set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException("Identity endpoint shouldn't be blank or null!", "IdentityEndpoint");
			}
			identityEndpoint = ((value.Last() == '/') ? value : (value + "/"));
		}
	}

	public string BearerToken
	{
		get
		{
			return bearerToken;
		}
		set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException("Bearer token shouldn't be blank or null!", "BearerToken");
			}
			bearerToken = value;
		}
	}

	public IWebProxy Proxy
	{
		get
		{
			return proxy;
		}
		private set
		{
			proxy = value ?? throw new ArgumentNullException("Proxy", "Proxy shouldn't be null!");
		}
	}

	public string CacheFolder
	{
		get
		{
			return cacheFolder;
		}
		private set
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException("CacheFolder shouldn't be blank or null!", "CacheFolder");
			}
			cacheFolder = value;
		}
	}

	public int RetriesCount { get; set; } = 3;


	public Func<TimeSpan> RetriesIntervalProvider { get; set; } = () => TimeSpan.FromSeconds(1.0);


	public BackendConfiguration(string apiEndpoint, string identityEndpoint, string idToken, IWebProxy proxy, string cacheFolder, Func<string, Task<string>> reissueToken = null, ILoggerProvider loggerProvider = null, bool disableConcurrency = false, bool byteRangeDownloadEnabled = true)
	{
		ApiEndpoint = apiEndpoint;
		IdentityEndpoint = identityEndpoint;
		BearerToken = idToken;
		Proxy = proxy;
		CacheFolder = cacheFolder;
		ReissueToken = reissueToken;
		LoggerProvider = loggerProvider;
		DisableConcurrency = disableConcurrency;
		ByteRangeDownloadEnabled = byteRangeDownloadEnabled;
	}
}
