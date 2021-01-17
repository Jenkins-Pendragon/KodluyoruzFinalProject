using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : Singleton<LevelManager>
{    
    public int LevelIndex
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentLevel", 1);
        }
        set
        {  
            PlayerPrefs.SetInt("CurrentLevel", value);
        }
    }
    public void NextLevel()
    {       
        StartCoroutine(NextLevelCo());
    }

    public void Restart()
    {
        StartCoroutine(LoadLevel());
    }

    public IEnumerator NextLevelCo() 
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Scene initScene = SceneManager.GetSceneAt(0);
        SceneManager.SetActiveScene(initScene);
        List<Scene> scenesToBeUnloaded = new List<Scene>();

        int sceneCount = SceneManager.sceneCount;

        //Add scenes to be unloaded to the list
        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name.Contains("Scene"))
            {
                scenesToBeUnloaded.Add(scene);
            }
        }

        //Unload all scenes that needs to be unloaded.
        foreach (var s in scenesToBeUnloaded)
        {
            yield return SceneManager.UnloadSceneAsync(s.buildIndex);
        }

        //Check if we can load this scene
        if (!Application.CanStreamedLevelBeLoaded(buildIndex))
        {
            //Set it back to default
            buildIndex = 2;
            LevelIndex = 1;
        }
        LevelIndex = buildIndex - 1;
        //Load the scene
        yield return SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
        Scene levelScene = SceneManager.GetSceneByBuildIndex(buildIndex);
        SceneManager.SetActiveScene(levelScene);
        PlayerPrefs.SetString("LastLevel", levelScene.name);        
        GameManager.Instance.StartGame();
    }

    public IEnumerator LoadLevel() 
    {
        string lastLevel = PlayerPrefs.GetString("LastLevel", "Scene01");
        yield return SceneManager.UnloadSceneAsync(lastLevel);
        yield return SceneManager.LoadSceneAsync(lastLevel, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(lastLevel));
        GameManager.Instance.StartGame();
    }
}
