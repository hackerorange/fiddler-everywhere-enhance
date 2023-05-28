using System;

namespace FiddlerBackend.Contracts;

[Flags]
public enum Roles
{
	None = 0,
	Read = 1,
	Create = 2,
	Update = 4,
	Delete = 8,
	All = 0xF
}
