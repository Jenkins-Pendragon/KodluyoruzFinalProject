using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlatformController : MonoBehaviour
{
    bool isAllPlatformEnded = false;
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

    private void Awake()
    {
        platformList[0].isPlatfromActive = true;
    }

    void CheckPlatformStatus()
    {
        if (isAllPlatformEnded)
            return;

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
        SetPlatformObjects(false);
        Vector3 pos = platformList[currentPlatform].pointA.position;
        pos.y = Player.transform.position.y;
        Sequence playerMovement = DOTween.Sequence();
        playerMovement.Append(Player.transform.DOMove(pos, 2f));
        playerMovement.Append(Player.transform.DORotate(platformList[currentPlatform].pointB.rotation.eulerAngles, 0.5f));
        playerMovement.Append(Player.transform.DOJump(platformList[currentPlatform].pointB.position, 1f, 1, 1f)).OnComplete(
            ()=> 
            {               
                currentPlatform += 1;
                if (currentPlatform == platformList.Count)
                {
                    isAllPlatformEnded = true;
                    EventManager.OnLevelSuccess.Invoke();
                }
                else
                    SetPlatformObjects(true);
            });
    }

    private void SetPlatformObjects(bool state) 
    {
        List<Enemy> enemyList = platformList[currentPlatform].enemyList;
        List<IShootable> shootables = platformList[currentPlatform].shootables;
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].NavMeshAgent.enabled = state;
        }

        for (int i = 0; i < shootables.Count; i++)
        {
            shootables[i].IsCanFire = state;
        }
    }
}
