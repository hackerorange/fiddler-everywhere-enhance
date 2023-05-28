using System;
using AutoMapper;
using FiddlerBackend.Contracts;
using FiddlerBackendSDK.ComposerCollections.Client;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Notifications.Serialization;
using Newtonsoft.Json.Linq;
using PubnubApi;

namespace FiddlerBackendSDK.Notifications.Pubnub;

public class PubnubNotificationSubscribeCallback : SubscribeCallback
{
	private readonly IObserver<NotificationMessage> clientObserver;

	private readonly INotificationMessageDeserializer messageDeserializer;

	private readonly IMapper mapper;

	private readonly IEntityCache<ComposerCollectionCacheItem> collectionsCache;

	public PubnubNotificationSubscribeCallback(IObserver<NotificationMessage> clientObserver, INotificationMessageDeserializer messageDeserializer, IMapper mapper, IEntityCache<ComposerCollectionCacheItem> collectionsCache)
	{
		this.mapper = mapper;
		this.clientObserver = clientObserver;
		this.messageDeserializer = messageDeserializer;
		this.collectionsCache = collectionsCache;
	}

	public override void Message<T>(Pubnub pubnub, PNMessageResult<T> message)
	{
		object obj = message.Message;
		JObject val = (JObject)((obj is JObject) ? obj : null);
		if (val == null)
		{
			return;
		}
		try
		{
			NotificationMessageDTO notificationMessageDTO = messageDeserializer.Deserialize(val);
			NotificationMessage notificationMessage = ((IMapperBase)mapper).Map<NotificationMessage>((object)notificationMessageDTO);
			if (notificationMessage == null)
			{
				return;
			}
			if (notificationMessage.Operation != "ResourceMoved")
			{
				clientObserver.OnNext(notificationMessage);
				return;
			}
			ResourceMovedNotificationMessageDTO resourceMovedNotificationMessageDTO = notificationMessageDTO as ResourceMovedNotificationMessageDTO;
			if (resourceMovedNotificationMessageDTO.ResourceType == ResourceType.ComposerCollection)
			{
				collectionsCache.Remove(resourceMovedNotificationMessageDTO.ResourceId);
			}
		}
		catch (Exception)
		{
		}
	}

	public override void Presence(Pubnub pubnub, PNPresenceEventResult presence)
	{
	}

	public override void Signal<T>(Pubnub pubnub, PNSignalResult<T> signal)
	{
	}

	public override void Status(Pubnub pubnub, PNStatus status)
	{
	}

	public override void ObjectEvent(Pubnub pubnub, PNObjectEventResult objectEvent)
	{
	}

	public override void MessageAction(Pubnub pubnub, PNMessageActionEventResult messageAction)
	{
	}

	public override void File(Pubnub pubnub, PNFileEventResult fileEvent)
	{
	}
}
