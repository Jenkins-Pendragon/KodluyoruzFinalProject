using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InitManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        //Init Game Here
        yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(2));        
        Destroy(gameObject);
    } 

    
}
