using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LevelFinal : MonoBehaviour
{

    PlatformController pc;
    public Transform finishPoint;
    public GameObject player;
    public GameObject finalGameCanvas;
    public GameObject enemyPrefab;

    private void OnEnable()
    {
        EventManager.OnLevelSuccess.AddListener(() => player.transform.DOMove(finishPoint.position, 1f));
        EventManager.OnLevelSuccess.AddListener(() => finalGameCanvas.SetActive(true));
        EventManager.OnLevelSuccess.AddListener(() => Instantiate(enemyPrefab, finishPoint.transform.position, Quaternion.Euler(transform.forward), enemyPrefab.transform));

    }

    private void OnDisable()
    {
        EventManager.OnLevelSuccess.RemoveListener(() => player.transform.DOMove(finishPoint.position, 1f));
        EventManager.OnLevelSuccess.RemoveListener(() => finalGameCanvas.SetActive(true));
        EventManager.OnLevelSuccess.RemoveListener(() => Instantiate(enemyPrefab, finishPoint.transform.position, Quaternion.Euler(transform.forward), enemyPrefab.transform));

    }


    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Level Success!");
          
        }

    }
}
