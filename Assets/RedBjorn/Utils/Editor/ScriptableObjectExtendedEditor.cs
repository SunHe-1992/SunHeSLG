using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.Utils
{
    [CustomEditor(typeof(ScriptableObjectExtended), editorForChildClasses: true)]
    [CanEditMultipleObjects]
    public class ScriptableObjectExtendedEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            ShowExtendedGUI();
        }

        public void ShowExtendedGUI()
        {
            var myTarget = (ScriptableObjectExtended)target;
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
