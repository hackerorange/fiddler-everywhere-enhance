using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Notifications;

namespace FiddlerBackendSDK.User;

public interface IUserClient
{
	Task<UserWithBestAccountDTO> GetCurrentUserAsync(bool includeOwnedAccounts = true);

	Task<long> GetCurrentUserNotificationsCounterAsync();

	Task<UserEventsResultDTO> GetEventsAsync(SearchUserEventsParameters searchParameters = null);

	Task<UserNotifications> GetNotificationsAsync(SearchUserEventsParameters searchParameters = null);

	Task<List<UserDiscountDTO>> GetUserDiscountsAsync();

	Task<UserDTO> SetEventsLastSeenAsync(EventType eventType);

	Task<UserDTO> SetNotificationsLastSeenAsync();

	Task<UserDTO> UpdateUserInfoAsync(UpdateUserDTO updateUser);

	Task<EventDTO> MarkEventAsReadAsync(Guid eventId);

	Task MarkNotificationAsReadAsync(EventNotificationMessage notificationMessage);

	AccountDTO GetBestAccount(UserWithBestAccountDTO user);
}
