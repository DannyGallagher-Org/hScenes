using System;

namespace Extensions
{
	public static class ArrayExtensions
	{
		public static void Swap<T>(this T[] array, int element, int element2) {
			var temp = array[element];
			array[element] = array[element2];
			array[element2] = temp;
		}
		
		public static T[] RemoveAt<T>(this T[] source, int index)
		{
			var dest = new T[source.Length - 1];
			if( index > 0 )
				Array.Copy(source, 0, dest, 0, index);

			if( index < source.Length - 1 )
				Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

			return dest;
		}

		public static T[,] Resize<T>(this T[,] original, int x, int y)
		{
			var newArray = new T[x, y];
			var minX = Math.Min(original.GetLength(0), newArray.GetLength(0));
			var minY = Math.Min(original.GetLength(1), newArray.GetLength(1));

			for (var i = 0; i < minY; ++i)
				Array.Copy(original, i * original.GetLength(0), newArray, i * newArray.GetLength(0), minX);

			return newArray;
		}
	}
}
