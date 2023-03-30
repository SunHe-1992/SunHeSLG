using System.Collections.Generic;

namespace RedBjorn.Utils
{
    public static class ListExtensions
    {
        /// <summary>
        /// Return random list value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T Random<T>(this IList<T> list)
        {
            if (list != null && list.Count > 0)
            {
                return list[UnityEngine.Random.Range(0, list.Count)];
            }
            return default(T);
        }

        public static T Random<T>(this IList<T> list, System.Random randomizer)
        {
            if (list != null && list.Count > 0 && randomizer != null)
            {
                return list[randomizer.Next(0, list.Count)];
            }
            return default(T);
        }

        public static void Shuffle<T>(this IList<T> list, System.Random randomizer)
        {
            if(list != null && randomizer != null)
            {
                var count = list.Count;
                for(int i = 0; i < count; i++)
                {
                    var index = randomizer.Next(i, count);
                    var element = list[index];
                    list.RemoveAt(index);
                    list.Insert(0, element);
                }
            }
        }
    }
}
