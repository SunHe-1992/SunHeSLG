using System;

namespace RedBjorn.Utils
{
    public static class ActionExtensions
    {
        /// <summary>
        /// Invoke action with null-check
        /// </summary>
        /// <param name="action"></param>
        public static void SafeInvoke(this Action action)
        {
            if (action != null)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Invoke action with null-check
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="context"></param>
        public static void SafeInvoke<T>(this Action<T> action, T context)
        {
            if (action != null)
            {
                action(context);
            }
        }

        /// <summary>
        /// Invoke action with null-check
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool SafeInvoke<T>(this Func<T, bool> action, T context)
        {
            if (action != null)
            {
                return action(context);
            }
            return false;
        }
    }
}
