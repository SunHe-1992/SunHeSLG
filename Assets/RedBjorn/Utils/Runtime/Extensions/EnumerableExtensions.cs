using System.Collections.Generic;

namespace RedBjorn.Utils
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Return string contains of rows. Each one contains one element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static string ToColumn<T>(this IEnumerable<T> enumerable)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var en in enumerable)
            {
                sb.AppendFormat("{0}\n", en);
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

    }
}