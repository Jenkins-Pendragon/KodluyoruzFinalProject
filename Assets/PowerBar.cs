using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PowerBar : MonoBehaviour
{
    public GameObject pointer;
    public bool isPowerStart = true;
    private RectTransform powerBar;
    float ForceSpeed = 0;
    public GameObject rootPanel;
    //sağ kırmızı z -31.6 ~ -44
    //sağ turuncu z -18.4 ~ -31.6
    //sağ sarı z -18.4 ~ -6.2
    //yeşil z -6.2 ~ 5.75

    private void Awake()
    {
        powerBar = pointer.GetComponent<RectTransform>();
    }


    private void OnEnable()
    {
        EventManager.OnLevelSuccess.AddListener(ActivatePowerPoint);
        EventManager.OnTapBar.AddListener(StopTheBar);
        EventManager.OnTapBar.AddListener(ForcePower);
        EventManager.OnTapBar.AddListener(() => rootPanel.SetActive(false));
        
    }

    private void OnDisable()
    {
        EventManager.OnLevelSuccess.RemoveListener(ActivatePowerPoint);
        EventManager.OnTapBar.RemoveListener(StopTheBar);
        EventManager.OnTapBar.RemoveListener(ForcePower);
        EventManager.OnTapBar.RemoveListener(() => rootPanel.SetActive(false));

    }



    public void StopTheBar()
    {
        isPowerStart = false;
        DOTween.KillAll();
    }


    private void ActivatePowerPoint()
    {
     
        StartCoroutine(PowerPoint());
    }

    public void ForcePower()
    {
        
        float angleZ = Mathf.Abs(powerBar.rotation.eulerAngles.z);

        if (angleZ > 300)
        {
            angleZ = 360 - angleZ;
        }

        if (angleZ >= 31)
        {
            RagdollController.force = 25f;
            Debug.Log("Kırmızı");
            Debug.Log(angleZ);
        }

       else if (angleZ >= 18)
        {
            RagdollController.force = 35f;
            Debug.Log("Turuncu");
            Debug.Log(angleZ);
        }

       else if (angleZ >= 6)
        {
            RagdollController.force = 45f;
            Debug.Log("Sarı");
            Debug.Log(angleZ);

        }

        else if (angleZ >= 0)
        {
            RagdollController.force = 300f;
            Debug.Log("Yeşil");
            Debug.Log(angleZ);
        }

      
       

        




    }

    IEnumerator PowerPoint()
    {
        while (isPowerStart)
        {
 
            pointer.transform.DORotate(new Vector3(0f, 0f, 40f), 1.25f).OnComplete(() => pointer.transform.DORotate(new Vector3(0f, 0f, -40f), 1.25f));
            yield return new WaitForSeconds(2.5f);

        }


        
    }

}
