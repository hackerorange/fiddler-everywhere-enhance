using System;
using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public interface IWorkspaceWithUsersDTO
{
	Guid Id { get; set; }

	IList<SharedWithUserDTO> SharedWithUsers { get; set; }
}
