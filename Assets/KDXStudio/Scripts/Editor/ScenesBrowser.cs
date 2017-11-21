#region Author
/************************************************************************************************************
Author: BODEREAU Roy
Website: http://roy-bodereau.fr/
GitHub: https://github.com/Kardux
LinkedIn: https://fr.linkedin.com/pub/roy-bodereau/b2/94/82b
************************************************************************************************************/
#endregion Author

#region Copyright
/************************************************************************************************************
CC-BY-SA 4.0
http://creativecommons.org/licenses/by-sa/4.0/
Cette oeuvre est mise a disposition selon les termes de la Licence Creative Commons Attribution 4.0
Partage dans les Memes Conditions 4.0 International.
************************************************************************************************************/
#endregion Copyright

namespace KDX.Editor
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;
    using UnityEditor.SceneManagement;

    public class ScenesBrowser : EditorWindow
    {
        #region Fields
        private Vector2 _windowScrollPosition;
        #endregion Fields

        #region EditorWindow
        [MenuItem("KDX/Editor Tools/Build scenes browser &S", false, 10)]
        private static void Init()
        {
            GetWindow(typeof(ScenesBrowser), false, "Scene browser");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            _windowScrollPosition = EditorGUILayout.BeginScrollView(_windowScrollPosition, false, false);

            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            for(int i = 0; i < scenes.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();

                if(i == 0)
                {
                    Color guiContentColor = GUI.contentColor;
                    if(Application.isPlaying == false)
                    {
                        GUI.contentColor = Color.blue;
                        if(GUILayout.Button("►", new GUILayoutOption[] { GUILayout.Width(24.0f) }))
                        {
                            EditorTools.PlayFromBuildInitialScene();
                        }
                    }
                    else
                    {
                        GUI.contentColor = Color.red;
                        if(GUILayout.Button("■", new GUILayoutOption[] { GUILayout.Width(24.0f) }))
                        {
                            EditorTools.PlayFromBuildInitialScene();
                        }
                    }
                    GUI.contentColor = guiContentColor;
                }
                else
                {
                    GUILayout.Space(32.0f);
                }

                Color guibackgroundColor = GUI.backgroundColor;
                if(EditorSceneManager.GetActiveScene().path == scenes[i].path)
                {
                    GUI.backgroundColor = Color.gray;
                }

                GUI.enabled = (Application.isPlaying == false && scenes[i].enabled);
                if(GUILayout.Button(i.ToString() + " - " + scenes[i].path.Split('/')[scenes[i].path.Split('/').Length - 1].Replace(".unity", "")))
                {
                    if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scenes[i].path);
                    }
                }
                GUI.enabled = true;

                GUI.backgroundColor = guibackgroundColor;

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        #endregion EditorWindow
    }
}