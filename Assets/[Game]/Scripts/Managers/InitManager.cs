using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using Sirenix.OdinInspector;

public class InitManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        //Init Game Here   
        yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);       
        yield return SceneManager.LoadSceneAsync(PlayerPrefs.GetString("LastLevel", "Level1"), LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(PlayerPrefs.GetString("LastLevel", "Level1")));
        GameManager.Instance.StartGame();
        Destroy(gameObject);
    } 

    [Button]
    public void ResetPlayerPrefs() 
    {
        PlayerPrefs.DeleteAll();
    }
}
