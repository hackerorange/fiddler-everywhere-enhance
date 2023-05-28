using System;

namespace FiddlerBackend.Contracts;

[Flags]
public enum JAMSessionCaptureSettings
{
	None = 0,
	Screenshots = 1,
	Console = 2,
	Cookies = 4,
	PostData = 8,
	DisableCache = 0x10,
	Video = 0x20,
	StorageInfo = 0x40,
	ClearBrowsingData = 0x80,
	CaptureFonts = 0x100,
	ReloadPage = 0x200,
	All = 0x3FF
}
