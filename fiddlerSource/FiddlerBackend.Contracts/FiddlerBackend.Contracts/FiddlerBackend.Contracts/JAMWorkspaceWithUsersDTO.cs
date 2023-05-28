using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class JAMWorkspaceWithUsersDTO : JAMWorkspaceDTO, IWorkspaceWithUsersDTO
{
	public IList<SharedWithUserDTO> SharedWithUsers { get; set; } = new List<SharedWithUserDTO>();

}
