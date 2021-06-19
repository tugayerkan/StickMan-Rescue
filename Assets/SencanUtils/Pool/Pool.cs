using System.Collections;
using System.Collections.Generic;
using SencanUtils.Pool.Factory;

namespace SencanUtils.Pool 
{
	public class Pool<T> : IEnumerable where T : IResettable
	{
		private List<T> members = new List<T>();
		private HashSet<T> unavailable = new HashSet<T>();
		private IFactory<T> factory;

		public Pool(IFactory<T> factory, int poolSize = 5)
		{
			this.factory = factory;

			for (int i = 0; i < poolSize; i++)
				Create();
		}

		public T Allocate() 
		{
			for(int i = 0; i < members.Count; i++) 
			{
				if(!unavailable.Contains(members[i])) 
				{
					unavailable.Add(members[i]);
					return members[i];
				}
			}
			T newMember = Create();
			unavailable.Add(newMember);
			return newMember;
		}

		public void Release(T member)
		{
			member.Reset();
			unavailable.Remove(member);
		}

		private T Create() 
		{
			T member = factory.Create();
			members.Add(member);
			return member;
		}

		public IEnumerator GetEnumerator()
		{
			return members.GetEnumerator();
		}
	}
}