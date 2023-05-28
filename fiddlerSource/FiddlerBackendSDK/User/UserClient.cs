using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Notifications;

namespace FiddlerBackendSDK.User;

public class UserClient : IUserClient
{
    public Task<UserWithBestAccountDTO> GetCurrentUserAsync(bool includeOwnedAccounts = true)
    {
        throw new NotImplementedException();
    }

    public Task<long> GetCurrentUserNotificationsCounterAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserEventsResultDTO> GetEventsAsync(SearchUserEventsParameters searchParameters = null)
    {
        throw new NotImplementedException();
    }

    public Task<UserNotifications> GetNotificationsAsync(SearchUserEventsParameters searchParameters = null)
    {
        var userNotifications = new UserNotifications
        {
            Notifications = new List<EventNotificationMessage>(),
            NonDeserializablePayloads = new List<string>(),
            TotalCount = 0,
            TotalUnreadCount = 0
        };
        return Task.FromResult<UserNotifications>(userNotifications);
    }

    public Task<List<UserDiscountDTO>> GetUserDiscountsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> SetEventsLastSeenAsync(EventType eventType)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> SetNotificationsLastSeenAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO> UpdateUserInfoAsync(UpdateUserDTO updateUser)
    {
        throw new NotImplementedException();
    }

    public Task<EventDTO> MarkEventAsReadAsync(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public Task MarkNotificationAsReadAsync(EventNotificationMessage notificationMessage)
    {
        throw new NotImplementedException();
    }

    public AccountDTO GetBestAccount(UserWithBestAccountDTO user)
    {
        throw new NotImplementedException();
    }
}