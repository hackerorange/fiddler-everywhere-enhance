using FiddlerBackend.Contracts;

namespace FiddlerBackendSDK.Quota;

public class QuotaClient : IQuotaClient
{
    public Task<QuotasUsageDTO> GetQuotaUsageAsync(Guid accountId)
    {
        var quotasUsageDto = new QuotasUsageDTO();
        var quotaValueDto = new QuotaValueDTO
        {
            Value = 0,
            Allowed = 1000000
        };

        quotasUsageDto.AccountId = accountId;
        quotasUsageDto.TotalAccountSize = quotaValueDto;
        quotasUsageDto.LocalSnapshots = quotaValueDto;
        quotasUsageDto.LocalAutoResponderRules = quotaValueDto;
        quotasUsageDto.LocalFilters = quotaValueDto;
        quotasUsageDto.SharedSnapshots = quotaValueDto;
        quotasUsageDto.SnapshotSize = quotaValueDto;
        quotasUsageDto.SnapshotSharedWithUsers = quotaValueDto;
        quotasUsageDto.SharedRuleSets = quotaValueDto;
        quotasUsageDto.RuleSetSharedWithUsers = quotaValueDto;
        quotasUsageDto.SharedComposerRequests = quotaValueDto;
        quotasUsageDto.ComposerRequestsSharedWithUsers = quotaValueDto;
        quotasUsageDto.LocalSessionsStatisticsLimit = quotaValueDto;
        quotasUsageDto.JAMSessionsCount = quotaValueDto;
        quotasUsageDto.AssignedSeats = quotaValueDto;
        quotasUsageDto.Viewers = quotaValueDto;
        return Task.FromResult(quotasUsageDto);
    }
}