using System;

namespace FiddlerBackend.Contracts;

public class UserWithBestAccountDTO : UserDTO
{
	public Guid? BestEverywhereAccountId { get; set; }

	public Guid? BestJamAccountId { get; set; }
}
