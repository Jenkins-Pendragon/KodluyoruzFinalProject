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
            StartCoroutine(NextPlatform());
        }
    }

    private IEnumerator NextPlatform()
    {
        SetPlatformObjects(false);
        yield return new WaitForSeconds(1f);
        Sequence playerMovement = DOTween.Sequence();
        if (platformList[currentPlatform].moveTo != null)
        {
            Vector3 pos = platformList[currentPlatform].moveTo.position;
            pos.y = Player.transform.position.y;
            playerMovement.Append(Player.transform.DOMove(pos, 2f).SetEase(Ease.Linear));
        }

        if (platformList[currentPlatform].jumpTo != null)
        {
            Player.transform.DORotate(platformList[currentPlatform].jumpTo.rotation.eulerAngles, 2f);
            playerMovement.Append(Player.transform.DOJump(platformList[currentPlatform].jumpTo.position, 1f, 1, 1f));
        }
        playerMovement.OnComplete(() =>
        {
            currentPlatform += 1;
            if (currentPlatform == platformList.Count)
            {
                Debug.Log("Level Succses");
                isAllPlatformEnded = true;
                EventManager.OnLevelSuccess.Invoke();
            }
            else
                SetPlatformObjects(true);
            CheckPlatformStatus();
            Debug.Log("Bitti");
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
