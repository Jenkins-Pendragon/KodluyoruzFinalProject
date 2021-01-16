using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        UIManager.instance.Assignments();
        SceneLoadStatement.isLoaded = true;
    }
    private void OnDisable()
    {
        SceneLoadStatement.isLoaded = false;
        UIManager.instance.ResetValues();
    }
}
