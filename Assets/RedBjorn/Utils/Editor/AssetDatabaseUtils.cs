using UnityEditor;
using UnityEngine;

namespace RedBjorn.Utils
{
    public static class AssetDatabaseUtils
    {
        public static T[] FindAssets<T>() where T : Object
        {
            var type = typeof(T);
            var guids = AssetDatabase.FindAssets(string.Concat("t:", typeof(T).Name));
            var assets = new T[guids.Length];
            for (int i = 0; i < assets.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                assets[i] = AssetDatabase.LoadAssetAtPath(path, type) as T;
            }
            return assets;
        }
    }
}
