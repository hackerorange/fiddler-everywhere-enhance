using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FiddlerBackendSDK.Core;

public class EntityCache<T> : IEntityCache<T> where T : IEntity
{
	private readonly ConcurrentDictionary<Guid, Lazy<Task<T>>> cache = new ConcurrentDictionary<Guid, Lazy<Task<T>>>();

	public async Task<T> GetAsync(Guid id, Func<Guid, Task<T>> populateAsync)
	{
		return await cache.GetOrAdd(id, (Guid id) => new Lazy<Task<T>>(populateAsync(id))).Value;
	}

	public void Add(T entity)
	{
		cache.TryAdd(entity.Id, new Lazy<Task<T>>(Task.FromResult(entity)));
	}

	public void AddRange(IEnumerable<T> entities)
	{
		foreach (T entity in entities)
		{
			Add(entity);
		}
	}

	public void Remove(Guid id)
	{
		cache.TryRemove(id, out var _);
	}
}
