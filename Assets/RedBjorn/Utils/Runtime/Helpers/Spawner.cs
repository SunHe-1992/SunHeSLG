using UnityEngine;

namespace RedBjorn.Utils
{
    public class Spawner
    {
        /// <summary>
        /// Spawn prefab
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <returns></returns>
        public static T Spawn<T>(T prefab) where T : Object
        {
            if (prefab != null)
            {
                return GameObject.Instantiate(prefab);
            }
            return null;
        }

        /// <summary>
        /// Spawn prefab and parent it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T Spawn<T>(T prefab, Transform parent) where T : Object
        {
            if (prefab != null)
            {
                return GameObject.Instantiate(prefab, parent);
            }
            return null;
        }


        /// <summary>
        /// Spawn prefab and set position and rotation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Object
        {
            if (prefab != null)
            {
                return GameObject.Instantiate(prefab, position, rotation);
            }
            return null;
        }

        /// <summary>
        /// Spawn prefab and parent it. Also set local position and rotation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="prefab"></param>
        /// <param name="localPosition"></param>
        /// <param name="localRotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static T Spawn<T>(T prefab, Vector3 localPosition, Quaternion localRotation, Transform parent) where T : Object
        {
            if (prefab != null)
            {
                return GameObject.Instantiate(prefab, localPosition, localRotation, parent);
            }
            return null;
        }


        /// <summary>
        /// Despawn gameobject
        /// </summary>
        /// <param name="gameobject"></param>
        public static void Despawn(GameObject gameobject)
        {
            GameObject.Destroy(gameobject);
        }

        /// <summary>
        /// Despawn gameobject with delay
        /// </summary>
        /// <param name="gameobject"></param>
        /// <param name="delay">in seconds</param>
        public static void Despawn(GameObject gameobject, float delay)
        {
            GameObject.Destroy(gameobject, delay);
        }
    }
}