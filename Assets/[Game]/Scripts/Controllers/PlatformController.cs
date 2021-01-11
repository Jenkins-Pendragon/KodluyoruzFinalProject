using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlatformController : MonoBehaviour
{
    public List<Platform> platformList = new List<Platform>();
    private int currentPlatform = 0;
    public GameObject Player;

    private void OnEnable()
    {
        EventManager.OnEnemyDie.AddListener(CheckPlatformStatus);
    }

    private void OnDisable()
    {
        EventManager.OnEnemyDie.RemoveListener(CheckPlatformStatus);
    }

    void CheckPlatformStatus()
    {
        List<Enemy> enemyList = platformList[currentPlatform].enemyList;
        bool isAllEnemiesDead = true;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (!enemyList[i].IsDead)
            {
                isAllEnemiesDead = false;
            }
        }

        if (isAllEnemiesDead)
        {
            NextPlatform();
        }
    }

    void NextPlatform()
    {
        Sequence playerMovement = DOTween.Sequence();
        playerMovement.Append(Player.transform.DOMove(platformList[currentPlatform].pointA.position, 2f));
        playerMovement.Append(Player.transform.DOJump(platformList[currentPlatform].pointB.position, 1f, 1, 1f));
        currentPlatform += 1;

        if(currentPlatform == platformList.Count)
        {
            EventManager.OnLevelSuccess.Invoke();
        }
    }
}
