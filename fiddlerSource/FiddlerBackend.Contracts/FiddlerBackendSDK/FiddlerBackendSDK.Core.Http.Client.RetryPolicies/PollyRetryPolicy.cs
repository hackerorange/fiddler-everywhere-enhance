using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Fallback;
using Polly.Retry;

namespace FiddlerBackendSDK.Core.Http.Client.RetryPolicies;

public static class PollyRetryPolicy
{
	public static AsyncPolicy<HttpResponseMessage> CreateHttpRetryPolicy(IBackendConfiguration backendConfiguration, ILogger logger)
	{
		AsyncFallbackPolicy<HttpResponseMessage> val = CreateExceptionThrowingFallbackPolicy(logger);
		AsyncRetryPolicy<HttpResponseMessage> val2 = CreateRetryPolicy(backendConfiguration);
		return (AsyncPolicy<HttpResponseMessage>)(object)Policy.WrapAsync<HttpResponseMessage>(new IAsyncPolicy<HttpResponseMessage>[2]
		{
			(IAsyncPolicy<HttpResponseMessage>)(object)val,
			(IAsyncPolicy<HttpResponseMessage>)(object)val2
		});
	}

	private static AsyncRetryPolicy<HttpResponseMessage> CreateRetryPolicy(IBackendConfiguration backendConfiguration)
	{
		return AsyncRetryTResultSyntax.WaitAndRetryAsync<HttpResponseMessage>(CreateHandledErrorsPolicyBuilder(), backendConfiguration.RetriesCount, (Func<int, TimeSpan>)((int _) => backendConfiguration.RetriesIntervalProvider()));
	}

	private static AsyncFallbackPolicy<HttpResponseMessage> CreateExceptionThrowingFallbackPolicy(ILogger logger)
	{
		return AsyncFallbackTResultSyntax.FallbackAsync<HttpResponseMessage>(CreateHandledErrorsPolicyBuilder(), (HttpResponseMessage)null, (Func<DelegateResult<HttpResponseMessage>, Task>)delegate(DelegateResult<HttpResponseMessage> res)
		{
			if (IsTransientNetworkError(res))
			{
				throw new TransientHttpException(res.Result.StatusCode);
			}
			LoggerExtensions.LogError(logger, res.Exception, "No network connection!", Array.Empty<object>());
			throw new NoNetworkConnectionException();
		});
	}

	private static PolicyBuilder<HttpResponseMessage> CreateHandledErrorsPolicyBuilder()
	{
		return Policy.HandleResult<HttpResponseMessage>((Func<HttpResponseMessage, bool>)((HttpResponseMessage res) => res != null && (res.StatusCode == HttpStatusCode.RequestTimeout || res.StatusCode > HttpStatusCode.InternalServerError))).Or<HttpRequestException>().OrInner<HttpRequestException>();
	}

	private static bool IsTransientNetworkError(DelegateResult<HttpResponseMessage> result)
	{
		return result.Result != null;
	}
}
