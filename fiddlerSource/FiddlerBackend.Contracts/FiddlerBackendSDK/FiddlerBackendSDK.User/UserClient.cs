using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.Core.Http.Client;
using FiddlerBackendSDK.Core.Http.Client.Validation;
using FiddlerBackendSDK.Notifications;
using FiddlerBackendSDK.Notifications.Serialization;
using FiddlerBackendSDK.Subscription;
using Microsoft.Extensions.Logging;

namespace FiddlerBackendSDK.User;

public class UserClient : IUserClient
{
	private readonly string usersRelativePath = "users";

	private readonly IFiddlerHttpClient fiddlerHttpClient;

	private readonly ISubscriptionClient subscriptionClient;

	private readonly INotificationMessageDeserializer notificationMessageDeserializer;

	private readonly ILogger<UserClient> logger;

	private readonly IValidationExceptionFactory exceptionFactory;

	private readonly IMapper mapper;

	public UserClient(IFiddlerHttpClient fiddlerHttpClient, ISubscriptionClient subscriptionClient, INotificationMessageDeserializer notificationMessageDeserializer, ILogger<UserClient> logger, IValidationExceptionFactory exceptionFactory, IMapper mapper)
	{
		this.fiddlerHttpClient = fiddlerHttpClient;
		this.subscriptionClient = subscriptionClient;
		this.notificationMessageDeserializer = notificationMessageDeserializer;
		this.logger = logger;
		this.exceptionFactory = exceptionFactory;
		this.mapper = mapper;
	}

	public async Task<UserWithBestAccountDTO> GetCurrentUserAsync(bool includeOwnedAccounts)
	{
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCode(HttpStatusCode.Unauthorized).Create();
		return await fiddlerHttpClient.GetAsync<UserWithBestAccountDTO>(usersRelativePath + (includeOwnedAccounts ? "?includeOwnedAccounts=true" : string.Empty), statusCodeValidator);
	}

	public async Task<long> GetCurrentUserNotificationsCounterAsync()
	{
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCode(HttpStatusCode.Unauthorized).Create();
		return await fiddlerHttpClient.GetAsync<long>(usersRelativePath + "/notifications-counter", statusCodeValidator);
	}

	public async Task<UserEventsResultDTO> GetEventsAsync(SearchUserEventsParameters searchParameters = null)
	{
		searchParameters = searchParameters ?? new SearchUserEventsParameters();
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCode(HttpStatusCode.Unauthorized).Create();
		return await fiddlerHttpClient.GetAsync<UserEventsResultDTO>("events?" + searchParameters.GetQueryString(), statusCodeValidator);
	}

	public async Task<UserNotifications> GetNotificationsAsync(SearchUserEventsParameters searchParameters = null)
	{
		return DeserializeEventPayloads(await GetEventsAsync(searchParameters));
	}

	public async Task<List<UserDiscountDTO>> GetUserDiscountsAsync()
	{
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCode(HttpStatusCode.Unauthorized).WithErrorCode(HttpStatusCode.NotFound)
			.Create();
		try
		{
			return await fiddlerHttpClient.GetAsync<List<UserDiscountDTO>>(usersRelativePath + "/discounts", statusCodeValidator);
		}
		catch (HttpException ex)
		{
			if (ex.HttpStatusCode == HttpStatusCode.NotFound)
			{
				return new List<UserDiscountDTO>();
			}
			throw ex;
		}
	}

	public async Task<UserDTO> SetEventsLastSeenAsync(EventType eventType)
	{
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCode(HttpStatusCode.OK).WithErrorCode(HttpStatusCode.Unauthorized).Create();
		return await fiddlerHttpClient.PatchAsync<UserDTO>($"{usersRelativePath}/events/last-seen/{eventType}", null, statusCodeValidator);
	}

	public async Task<UserDTO> SetNotificationsLastSeenAsync()
	{
		return await SetEventsLastSeenAsync(EventType.Notification);
	}

	public async Task<UserDTO> UpdateUserInfoAsync(UpdateUserDTO updateUser)
	{
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCodes(HttpStatusCode.OK, HttpStatusCode.NotModified).WithErrorCodes(HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized).Create();
		return await fiddlerHttpClient.PatchAsync<UpdateUserDTO, UserDTO>(usersRelativePath, updateUser, statusCodeValidator);
	}

	public async Task<EventDTO> MarkEventAsReadAsync(Guid eventId)
	{
		IFiddlerHttpStatusCodeValidator statusCodeValidator = new FiddlerBackendSDK.Core.Http.Client.Validation.FiddlerHttpStatusCodeValidator.Builder(exceptionFactory).WithSuccessCodes(HttpStatusCode.OK, HttpStatusCode.NotModified).WithErrorCodes(HttpStatusCode.NotFound, HttpStatusCode.Unauthorized).Create();
		UpdateEventDTO resource = new UpdateEventDTO
		{
			IsRead = true
		};
		return await fiddlerHttpClient.PatchAsync<UpdateEventDTO, EventDTO>($"events/{eventId}", resource, statusCodeValidator);
	}

	public async Task MarkNotificationAsReadAsync(EventNotificationMessage notificationMessage)
	{
		notificationMessage.IsRead = (await MarkEventAsReadAsync(notificationMessage.EventId)).IsRead;
	}

	public AccountDTO GetBestAccount(UserWithBestAccountDTO user)
	{
		return user.Accounts.FirstOrDefault((UserAccountDTO x) => true);
	}

	private UserNotifications DeserializeEventPayloads(UserEventsResultDTO userEvents)
	{
		UserNotifications userNotifications = new UserNotifications
		{
			TotalCount = userEvents.TotalCount,
			TotalUnreadCount = userEvents.TotalUnreadCount,
			Notifications = new List<EventNotificationMessage>(),
			NonDeserializablePayloads = new List<string>()
		};
		foreach (EventDTO @event in userEvents.Events)
		{
			try
			{
				NotificationMessageDTO notificationMessageDTO = notificationMessageDeserializer.Deserialize(@event.Payload);
				NotificationMessage message = ((IMapperBase)mapper).Map<NotificationMessage>((object)notificationMessageDTO);
				userNotifications.Notifications.Add(new EventNotificationMessage
				{
					Message = message,
					EventId = @event.Id,
					IsRead = @event.IsRead
				});
			}
			catch (Exception ex)
			{
				userNotifications.NonDeserializablePayloads.Add(@event.Payload);
				LoggerExtensions.LogError((ILogger)(object)logger, ex, $"Error deserializing event with id: {@event.Id}, payload: {@event.Payload}", Array.Empty<object>());
			}
		}
		return userNotifications;
	}
}
