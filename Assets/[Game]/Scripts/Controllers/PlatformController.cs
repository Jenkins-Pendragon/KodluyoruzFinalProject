using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class PlatformController : MonoBehaviour
{    
    bool isAllPlatformEnded = false;
    public List<Platform> platformList = new List<Platform>();
    private int currentPlatform = 0;
    
    private void OnEnable()
    {
        EventManager.OnEnemyDie.AddListener(CheckPlatformStatus);
        EventManager.OnLevelStart.AddListener(() => SetPlatformObjects(true));
        EventManager.OnPlayerDeath.AddListener(()=> SetShootables(false));
    }

    private void OnDisable()
    {
        EventManager.OnEnemyDie.RemoveListener(CheckPlatformStatus);
        EventManager.OnLevelStart.RemoveListener(() => SetPlatformObjects(true));
        EventManager.OnPlayerDeath.RemoveListener(()=> SetShootables(false));
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

        if (isAllEnemiesDead && !PlayerData.Instance.IsPlayerDead)
        {
            StartCoroutine(NextPlatform());
        }
    }

    private IEnumerator NextPlatform()
    {
        PlayerData.Instance.IsImmune = true;              
        SetPlatformObjects(false);
        yield return new WaitForSeconds(1f);
        Sequence playerMovement = DOTween.Sequence();
        if (platformList[currentPlatform].moveTo != null)
        {
            Vector3 pos = platformList[currentPlatform].moveTo.position;
            pos.y = PlayerData.Instance.transform.position.y;
            playerMovement.Append(PlayerData.Instance.transform.DOMove(pos, 2f).SetEase(Ease.Linear));
        }

        if (platformList[currentPlatform].jumpTo != null)
        {
            PlayerData.Instance.transform.DORotate(platformList[currentPlatform].jumpTo.rotation.eulerAngles, 2f);
            playerMovement.Append(PlayerData.Instance.transform.DOJump(platformList[currentPlatform].jumpTo.position, 1f, 1, 1f));
        }
        playerMovement.OnComplete(() =>
        {
            currentPlatform += 1;
            if (currentPlatform == platformList.Count)
            {
                if (PlayerData.Instance.IsPlayerDead) return; 
                isAllPlatformEnded = true;                
                EventManager.OnFinishLine.Invoke();
            }
            else
                SetPlatformObjects(true);

            CheckPlatformStatus();            
            PlayerData.Instance.IsImmune = false;
        });
    }

    private void SetPlatformObjects(bool state) 
    {
        SetEnemies(state);
        SetShootables(state);
    }

    private void SetEnemies(bool state) 
    {
        List<Enemy> enemyList = platformList[currentPlatform].enemyList;
        for (int i = 0; i < enemyList.Count; i++)
        {
            if (enemyList[i].NavMeshAgent != null && enemyList[i].canRun && !enemyList[i].IsDead && enemyList[i].IsInteractable) enemyList[i].NavMeshAgent.enabled = state;
        }
    }

    private void SetShootables(bool state) 
    {
        List<IShootable> shootables = platformList[currentPlatform].shootables;
        for (int i = 0; i < shootables.Count; i++)
        {
            shootables[i].IsCanFire = state;
        }
    }
}
