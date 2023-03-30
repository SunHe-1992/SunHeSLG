using System.Collections.Generic;

namespace RedBjorn.Utils
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Try get value. If none default will be returned
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue TryGetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            TValue v;
            if (dict.TryGetValue(key, out v))
            {
                return v;
            }
            return default(TValue);
        }

        /// <summary>
        /// Try get value. If none default will be added to dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TValue TryGetOrCreate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TValue : new()
        {
            TValue v;
            if (dict.TryGetValue(key, out v))
            {
                return v;
            }
            v = new TValue();
            dict.Add(key, v);
            return v;
        }
    }
}
