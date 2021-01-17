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
    private Stack<GameObject> stackCoins;
    public RectTransform targetRect;
    public WaitForSeconds coinWaitTime = new WaitForSeconds(0.08f);
    public WaitForSeconds textWaitTime = new WaitForSeconds(0.05f);
    public Text moneyText;
    private Vector3 defaultPos;
    public float multiplier = 5;
    int currentVal;
    private void Awake()
    {
        stackCoins = new Stack<GameObject>(coins);
        Instance = this;
        defaultPos = coins[0].transform.position;
    }
    private void OnEnable()
    {
        EventManager.OnEnemyDie.AddListener(SetScore);
        EventManager.OnGameStarted.AddListener(ResetValues);
        EventManager.OnCalculateScoreMultiplier.AddListener(ScoreMultiplier);
    }
    private void OnDisable()
    {
        EventManager.OnEnemyDie.RemoveListener(SetScore);
        EventManager.OnGameStarted.RemoveListener(ResetValues);
        EventManager.OnCalculateScoreMultiplier.RemoveListener(ScoreMultiplier);
    }
    private void ResetValues()
    {
        currentVal = 0;
        moneyText.text = "0";
    }
    // Golden Enemy ' nin çarpış Anında

    public void ScoreMultiplier() 
    {
        StartCoroutine(OpenAndGoAsync());
    }   
    IEnumerator OpenAndGoAsync()
    {
        //int newScore = Convert.ToInt32(currentVal * multiplier);
        //for (int i = 0; i < newScore; i+=2)
        //{
        //    currentVal += 2;
        //    if (currentVal >= newScore) currentVal = newScore;            
        //    GameObject coin = stackCoins.Pop();
        //    if (coin != null)
        //    {
        //        coin.SetActive(true);
        //        coin.transform.DOMove(targetRect.transform.position, 0.08f).OnComplete(() =>
        //        {                    
        //            moneyText.text = currentVal.ToString();                    
        //            stackCoins.Push(coin);
        //            coin.SetActive(false);
        //            coin.transform.position = defaultPos;
        //        });
        //    }
        //    else moneyText.text = currentVal.ToString();
        //    yield return coinWaitTime;
        //}
        //yield return new WaitForSeconds(0.5f);
        //EventManager.OnLevelSuccess.Invoke();
        int newScore = Convert.ToInt32(currentVal * multiplier);        
        int value = Mathf.FloorToInt((float)(newScore - currentVal) / (float)coins.Count);
        Debug.Log(newScore);
        Debug.Log(value);
        foreach (GameObject coin in coins)
        {
            coin.SetActive(true);
            coin.transform.DOMove(targetRect.transform.position, 0.3f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                currentVal += value;
                moneyText.text = currentVal.ToString();
                coin.transform.position = defaultPos;
                coin.SetActive(false);
                Debug.Log(currentVal);
            });
            yield return coinWaitTime;
        }        
        yield return new WaitForSeconds(0.5f);
        currentVal = newScore;
        moneyText.text = currentVal.ToString();
        yield return new WaitForSeconds(0.5f);
        EventManager.OnLevelSuccess.Invoke();
    }
    public void SetScore()
    {
        StartCoroutine(SetScoreCo());
    }
    public IEnumerator SetScoreCo()
    {
        for (int i = 0; i < 3; i++)
        {
            currentVal += 1;
            yield return textWaitTime;
            moneyText.text = currentVal.ToString();
        }
    }
}

