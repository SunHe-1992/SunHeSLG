using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.Utils
{
    [CustomEditor(typeof(MonoBehaviourExtended), editorForChildClasses: true)]
    [CanEditMultipleObjects]
    public class MonoBehaviourExtendedEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var myTarget = (MonoBehaviourExtended)target;
            var methodButtons = myTarget.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                        .Where(m => m.IsDefined(typeof(MethodButtonAttribute), false))
                                        .OrderBy(m => m.Name);
            foreach (var m in methodButtons)
            {
                if (GUILayout.Button(m.Name))
                {
                    m.Invoke(target, null);
                }
            }
        }

    }
}
