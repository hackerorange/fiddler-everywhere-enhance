using System;
using AutoMapper;
using FiddlerBackendSDK.ComposerCollections.Client;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Notifications.Serialization;
using PubnubApi;

namespace FiddlerBackendSDK.Notifications.Pubnub;

public class PubnubChannelObservable : IObservable<NotificationMessage>
{
	private class PubnubUnsubscriber : IDisposable
	{
		private readonly IPubnubClient pubnub;

		private readonly PubnubNotificationSubscribeCallback pubnubNotificationSubscribeCallback;

		private readonly string channelName;

		public PubnubUnsubscriber(IPubnubClient pubnub, string channelName, PubnubNotificationSubscribeCallback pubnubNotificationSubscribeCallback)
		{
			this.pubnub = pubnub;
			this.channelName = channelName;
			this.pubnubNotificationSubscribeCallback = pubnubNotificationSubscribeCallback;
		}

		public void Dispose()
		{
			pubnub.RemoveListener((SubscribeCallback)(object)pubnubNotificationSubscribeCallback);
			pubnub.Unsubscribe(channelName);
		}
	}

	private readonly IPubnubClient pubnubClient;

	private readonly INotificationMessageDeserializer messageDeserializer;

	private readonly IMapper mapper;

	private readonly IEntityCache<ComposerCollectionCacheItem> collectionsCache;

	private readonly string channelName;

	public PubnubChannelObservable(IPubnubClient pubnubClient, INotificationMessageDeserializer messageDeserializer, IMapper mapper, IEntityCache<ComposerCollectionCacheItem> collectionsCache, string channelName)
	{
		this.pubnubClient = pubnubClient;
		this.channelName = channelName;
		this.messageDeserializer = messageDeserializer;
		this.mapper = mapper;
		this.collectionsCache = collectionsCache;
	}

	public IDisposable Subscribe(IObserver<NotificationMessage> observer)
	{
		PubnubNotificationSubscribeCallback pubnubNotificationSubscribeCallback = new PubnubNotificationSubscribeCallback(observer, messageDeserializer, mapper, collectionsCache);
		pubnubClient.AddListener((SubscribeCallback)(object)pubnubNotificationSubscribeCallback);
		pubnubClient.Subscribe(channelName);
		return new PubnubUnsubscriber(pubnubClient, channelName, pubnubNotificationSubscribeCallback);
	}
}
