using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
// Enemy On Die 

public class Announcer : MonoBehaviour
{
    public static Announcer Instance;
    public List<GameObject> coins;
    public RectTransform targetRect;
    public WaitForSeconds coinWaitTime = new WaitForSeconds(0.08f);
    public WaitForSeconds textWaitTime = new WaitForSeconds(0.05f);
    public Text moneyText;
    private Vector3 defaultPos;
    public float multiplier = 5;
    int currentVal;
    private void Awake()
    {
        Instance = this;
        defaultPos = coins[0].transform.position;
    }
    private void OnEnable()
    {
        EventManager.OnEnemyDie.AddListener(WhenEnemyDie);
        EventManager.OnGameStarted.AddListener(ResetValues);
    }
    private void OnDisable()
    {
        EventManager.OnEnemyDie.RemoveListener(WhenEnemyDie);
        EventManager.OnGameStarted.RemoveListener(ResetValues);
    }
    private void ResetValues()
    {
        currentVal = 0;
        moneyText.text = "0";
    }
    // Golden Enemy ' nin çarpış Anında
    public void OpenAndGo()
    {
        StartCoroutine(OpenAndGoAsync());
    }
    IEnumerator OpenAndGoAsync()
    {
        foreach (GameObject x in coins)
        {
            yield return coinWaitTime;
            x.SetActive(true);
            x.transform.DOMove(targetRect.transform.position, 0.3f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                float value = currentVal;
                value *= multiplier / 10;
                moneyText.text = value.ToString();
                x.transform.position = defaultPos;
                x.SetActive(false);
            });
        }
    }
    public void WhenEnemyDie()
    {
        StartCoroutine(WhenEnemyDieAsync());
    }
    public IEnumerator WhenEnemyDieAsync()
    {
        for (int i = 0; i < 3; i++)
        {
            currentVal += 1;
            yield return textWaitTime;
            moneyText.text = currentVal.ToString();
        }
    }
}

