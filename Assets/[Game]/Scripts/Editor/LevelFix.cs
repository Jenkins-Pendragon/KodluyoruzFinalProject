using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;



[InitializeOnLoad]
public class LevelFix
{    
    static LevelFix()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

  

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            bool initCheck = true;
            int sceneCount = SceneManager.sceneCount;            
            string scenePath = "Assets/[Game]/Scenes/Init.unity";
            //string scenePath = AssetDatabase.GetAssetPath(init);


            for (int i = 0; i < sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.name.Contains("Init"))
                {
                    initCheck = false;                    
                    break;
                }
            }

            if (!initCheck)
            {                
                SetPlayModeStartScene(scenePath);
            }
        }
    }

    private static void SetPlayModeStartScene(string scenePath)
    {
        SceneAsset myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        if (myWantedStartScene != null)
        {
            EditorSceneManager.playModeStartScene = myWantedStartScene;
        }
        else Debug.Log("Could not find Scene " + scenePath);
    }


}
