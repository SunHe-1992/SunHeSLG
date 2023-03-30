using System;
using UnityEditor;
using UnityEngine;

namespace RedBjorn.Utils
{
    public class ConfirmEditorWindow : EditorWindow
    {

        string Question;
        string YesText;
        string NoText;
        Action YesAction;
        Action NoAction;


        /// <summary>
        /// Show yes/no window
        /// </summary>
        /// <param name="question">plain text</param>
        /// <param name="yesText"></param>
        /// <param name="noText"></param>
        /// <param name="yesAction">left button action</param>
        /// <param name="noAction">right button action</param>
        public static void Init(string question, string yesText = "OK", string noText = "Cancel", Action yesAction = null, Action noAction = null)
        {
            var window = (ConfirmEditorWindow)GetWindow(typeof(ConfirmEditorWindow));
            window.minSize = new Vector2(200f, 200f);
            window.maxSize = new Vector2(200f, 200f);
            window.titleContent = new GUIContent("Confirm");
            window.YesAction = yesAction;
            window.NoAction = noAction;
            window.YesText = yesText;
            window.NoText = noText;
            window.Question = question;
            window.CenterOnMainWin();
            window.Show();
        }

        void OnGUI()
        {
            Draw();
        }

        void Draw()
        {
            GUILayout.Label(Question, GUILayout.MinWidth(200f), GUILayout.MaxWidth(200f), GUILayout.ExpandHeight(true));
            GUILayout.BeginHorizontal();

            if (YesAction != null)
            {
                if (GUILayout.Button(YesText))
                {
                    Close();
                    YesAction();
                }
            }

            if (GUILayout.Button(NoText))
            {
                Close();
                if (NoAction != null)
                {
                    NoAction();
                }
            }

            GUILayout.EndHorizontal();
        }
    }
}