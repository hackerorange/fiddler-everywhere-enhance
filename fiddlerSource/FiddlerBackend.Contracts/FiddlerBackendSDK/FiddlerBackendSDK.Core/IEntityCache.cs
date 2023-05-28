using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Core;

public interface IEntityCache<T> where T : IEntity
{
	Task<T> GetAsync(Guid id, Func<Guid, Task<T>> populateAsync);

	void Add(T entity);

	void AddRange(IEnumerable<T> entities);

	void Remove(Guid id);
}
