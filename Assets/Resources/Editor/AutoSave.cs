using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AutoSave
{
    private const double saveDelayTime = 60.0;
    private static double savedTime { get { return EditorPrefs.GetFloat("last_saved_time"); } set { EditorPrefs.SetFloat("last_saved_time", (float)value); } }

    static AutoSave()
    {
        EditorApplication.delayCall += DelayCalled;
        EditorApplication.update += Update;

        savedTime = EditorApplication.timeSinceStartup;
    }

    private static void Update()
    {
        if (!EditorApplication.isPlaying)
        {
            if (EditorApplication.timeSinceStartup - savedTime >= saveDelayTime)
            {
                savedTime = EditorApplication.timeSinceStartup;

                EditorSceneManager.SaveOpenScenes();
                EditorApplication.SaveAssets();
            }
        }
    }

    private static void DelayCalled()
    {
        if (!EditorApplication.isPlaying)
        {
            savedTime = EditorApplication.timeSinceStartup;

            EditorSceneManager.SaveOpenScenes();
            EditorApplication.SaveAssets();
        }
    }
}