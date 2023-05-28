using System;
using System.Collections.Generic;
using AutoMapper;
using FiddlerBackendSDK.ComposerCollections.Client;
using FiddlerBackendSDK.Core;
using FiddlerBackendSDK.Notifications.ChannelNaming;
using FiddlerBackendSDK.Notifications.Pubnub;
using FiddlerBackendSDK.Notifications.Serialization;

namespace FiddlerBackendSDK.Notifications;

public class NotificationObservable : IObservable<NotificationMessage>
{
	private class NotificationUnsubscriber : IDisposable
	{
		private readonly ICollection<IObserver<NotificationMessage>> observers;

		private readonly IObserver<NotificationMessage> observer;

		private readonly IDisposable pubnubObservableDisposable;

		public NotificationUnsubscriber(ICollection<IObserver<NotificationMessage>> observers, IObserver<NotificationMessage> observer, IDisposable pubnubObservableDisposable)
		{
			this.observers = observers;
			this.observer = observer;
			this.pubnubObservableDisposable = pubnubObservableDisposable;
		}

		public void Dispose()
		{
			if (observer != null)
			{
				observers.Remove(observer);
			}
			pubnubObservableDisposable.Dispose();
		}
	}

	private readonly PubnubChannelObservable pubnubChannelObservable;

	private readonly ICollection<IObserver<NotificationMessage>> observers;

	public NotificationObservable(IPubnubClient pubnubClient, INotificationMessageDeserializer messageDeserializer, IUserChannelNameCreator userChannelNameCreator, IMapper mapper, IEntityCache<ComposerCollectionCacheItem> collectionsCache, string userEmail)
	{
		string channelName = userChannelNameCreator.CreateUniqueChannelName(userEmail);
		pubnubChannelObservable = new PubnubChannelObservable(pubnubClient, messageDeserializer, mapper, collectionsCache, channelName);
		observers = new List<IObserver<NotificationMessage>>();
	}

	public IDisposable Subscribe(IObserver<NotificationMessage> observer)
	{
		if (!observers.Contains(observer))
		{
			observers.Add(observer);
		}
		IDisposable pubnubObservableDisposable = pubnubChannelObservable.Subscribe(observer);
		return new NotificationUnsubscriber(observers, observer, pubnubObservableDisposable);
	}
}
