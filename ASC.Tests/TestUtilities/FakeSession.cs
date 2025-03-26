using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Tests.TestUtilities
{
	public class FakeSession : ISession
	{
		public bool IsAvailable => true;
		public string Id => Guid.NewGuid().ToString();
		public IEnumerable<string> Keys => sessionFactory.Keys;

		private Dictionary<string, byte[]> sessionFactory = new Dictionary<string, byte[]>();

		public void Clear() => sessionFactory.Clear();

		public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

		public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

		public bool TryGetValue(string key, out byte[] value) => sessionFactory.TryGetValue(key, out value);

		public void Set(string key, byte[] value) => sessionFactory[key] = value;

		public void Remove(string key) => sessionFactory.Remove(key);
	}
}

