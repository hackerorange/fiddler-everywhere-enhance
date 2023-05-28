using System.Collections.Generic;

namespace FiddlerBackend.Contracts;

public class DeletedObjectsS3DTO
{
	public List<string> DeletedKeys { get; set; } = new List<string>();


	public IList<DeleteErrorS3DTO> DeleteErrors { get; set; } = new List<DeleteErrorS3DTO>();

}
