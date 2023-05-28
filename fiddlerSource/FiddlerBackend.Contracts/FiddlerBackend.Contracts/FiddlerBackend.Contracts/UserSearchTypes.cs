using System;

namespace FiddlerBackend.Contracts;

[Flags]
public enum UserSearchTypes
{
	Sender = 1,
	Receiver = 2,
	SenderOrReceiver = 3
}
