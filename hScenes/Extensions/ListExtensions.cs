using System;
using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
	public static class ListExtensions
	{
		public static void Swap<T>(this List<T> list, int index, int index2)
		{
			var element = list[index];
			list[index] = list[index2];
			list[index2] = element;
		}

		public static T Random<T>(this IEnumerable<T> enumerable)
		{
			if (enumerable == null)
			{
				throw new ArgumentNullException(nameof(enumerable));
			}

			// note: creating a Random instance each call may not be correct for you,
			// consider a thread-safe static instance
			var r = new Random();  
			var list = enumerable as IList<T> ?? enumerable.ToList(); 
			return list.Count == 0 ? default(T) : list[r.Next(0, list.Count)];
		}
		
		public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate) 
		{
			if (items == null) throw new ArgumentNullException("items");
			if (predicate == null) throw new ArgumentNullException("predicate");

			var retVal = 0;
			foreach (var item in items) {
				if (predicate(item)) return retVal;
				retVal++;
			}
			return -1;
		}

		public static void Resize<T>(this List<T> list, int sz, T c)
		{
			var cur = list.Count;
			if(sz < cur)
				list.RemoveRange(sz, cur - sz);
			else if(sz > cur)
			{
				if(sz > list.Capacity)//this bit is purely an optimisation, to avoid multiple automatic capacity changes.
					list.Capacity = sz;
				list.AddRange(Enumerable.Repeat(c, sz - cur));
			}
		}
		
		public static void Resize<T>(this List<T> list, int sz) where T : new()
		{
			Resize(list, sz, new T());
		}
	}
}
