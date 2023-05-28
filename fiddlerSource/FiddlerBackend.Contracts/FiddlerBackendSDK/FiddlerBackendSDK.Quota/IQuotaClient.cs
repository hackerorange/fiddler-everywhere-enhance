using System;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.Quota;

public interface IQuotaClient
{
	Task<QuotasUsageDTO> GetQuotaUsageAsync(Guid accountId);
}
