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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Reflection;
    using UnityEditor;
    using UnityEngine;
    using UnityEditor.SceneManagement;

    public class EditorTools
    {
        #region Menu Items
        [MenuItem("KDX/Editor Tools/Play-Stop, scene at build index = 0 &P", false, 0)]
        public static void PlayFromBuildInitialScene()
        {
            if(EditorApplication.isPlaying == true)
            {
                EditorApplication.isPlaying = false;
                EditorApplication.playmodeStateChanged += OpenPreviousScene;
            }
            else
            {
                if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == false)
                {
                    return;
                }

                EditorPrefs.SetString("PreviousScenePath", EditorSceneManager.GetActiveScene().path);
                EditorSceneManager.OpenScene(GetScenePaths()[0]);
                EditorApplication.isPlaying = true;
            }
        }

        [MenuItem("KDX/Editor Tools/Clear PlayerPrefs", false, 100)]
        static private void ClearPlayerPrefs()
        {
            if(EditorUtility.DisplayDialog("Clear PlayerPrefs?", "Do you want to delete the PlayerPrefs?\n(This can not be undone)", "Delete", "Cancel"))
            {
                PlayerPrefs.DeleteAll();
                EditorUtility.DisplayDialog("PlayerPrefs cleared!", "PlayerPrefs successfully deleted!", "OK");
            }
        }

        [MenuItem("KDX/Editor Tools/Find missing scripts (recursively)", false, 101)]
        static private void FindMissingScriptsRecursively()
        {
            ClearConsole();
            List<GameObject> gameObjectWithMissingScript = new List<GameObject>();
            GameObject[] go = Selection.gameObjects;
            for(int i = 0; i < go.Length; i++)
            {
                FindMissingScriptInGameObject_r(go[i], ref gameObjectWithMissingScript);
            }
            Selection.objects = gameObjectWithMissingScript.ToArray();
            Debug.Log(string.Format(gameObjectWithMissingScript.Count + " missing script(s) found."));
        }

        private static void FindMissingScriptInGameObject_r(GameObject _objectToSearch, ref List<GameObject> _missingScriptsGameObjectsList)
        {
            Component[] components = _objectToSearch.GetComponents<Component>();
            bool nullComponentFound = false;
            for(int i = 0; i < components.Length; i++)
            {
                if(components[i] == null)
                {
                    nullComponentFound = true;
                    Debug.Log("\"" + _objectToSearch.name + "\" has an empty script attached in position " + i, _objectToSearch);
                }
            }

            if(nullComponentFound)
            {
                _missingScriptsGameObjectsList.Add(_objectToSearch);
            }

            foreach(Transform child in _objectToSearch.transform)
            {
                FindMissingScriptInGameObject_r(child.gameObject, ref _missingScriptsGameObjectsList);
            }
        }
        #endregion Menu Items

        #region Static Methods
        #region Public
        public static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            Type logEntries = assembly.GetType("UnityEditorInternal.LogEntries");
            MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear");
            clearConsoleMethod.Invoke(new object(), null);
        }

        public static string[] GetScenePaths(bool _includeDisabled = false)
        {
            List<string> sceneNames = new List<string>();

            foreach(UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes)
            {

                if(scene.enabled || _includeDisabled)
                {
                    sceneNames.Add(scene.path);
                }
            }
            return sceneNames.ToArray();
        }
        #endregion Public

        #region Private
        private static void OpenPreviousScene()
        {
            if(EditorPrefs.HasKey("PreviousScenePath"))
            {
                EditorSceneManager.OpenScene(EditorPrefs.GetString("PreviousScenePath"));
            }
            EditorApplication.playmodeStateChanged -= OpenPreviousScene;
        }
        #endregion Private
        #endregion Static Methods
    }
}