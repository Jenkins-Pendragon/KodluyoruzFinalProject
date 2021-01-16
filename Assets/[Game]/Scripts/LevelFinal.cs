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
    public GameObject powerCanvas;


    private void OnEnable()
    {
        EventManager.OnLastPlatform.AddListener(FinalPrepare);
        EventManager.OnLastPlatform.AddListener(EnablePowerCanvas);


    }


    private void OnDisable()
    {
        EventManager.OnLastPlatform.RemoveListener(FinalPrepare);
        EventManager.OnLastPlatform.RemoveListener(EnablePowerCanvas);

    }


    private void EnablePowerCanvas()
    {
        powerCanvas.SetActive(true);
    }


    private void FinalPrepare()
    {
        player.transform.DOMove(finishPoint.position, 1f);
        finalGameCanvas.SetActive(true);
        //Instantiate(enemyPrefab, transform.position + Vector3.forward, Quaternion.identity);

    }

    private void OnTriggerEnter(Collider other)
    {

        EventManager.OnLevelSuccess.Invoke();
        
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Level Success!");
          
        }

    }
}
