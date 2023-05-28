using System;

namespace FiddlerBackend.Contracts;

[Flags]
public enum NotificationServiceTypes
{
	None = 0,
	Email = 1,
	Pubnub = 2,
	Events = 4,
	EventsAndPubnub = 6,
	All = 7
}
