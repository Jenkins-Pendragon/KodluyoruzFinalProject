using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScorePlatform : MonoBehaviour
{
    public float multiplier;
    public Transform[] confettiPositions;
    public Material highlightedMaterial;
    private MeshRenderer meshRenderer; 
    public MeshRenderer MeshRenderer { get { return (meshRenderer == null) ? meshRenderer = GetComponent<MeshRenderer>() : meshRenderer; } }
    public Material defaultMaterial;    
    bool canSwitch = true;
    public void OnCollisionEnter(Collision collision)
    {
        GoldenEnemyRagdoll enemyRagdoll = collision.gameObject.GetComponent<GoldenEnemyRagdoll>();
        if (enemyRagdoll != null)
        {
            for (int i = 0; i < confettiPositions.Length; i++)
            {
                Confetti.Instance.PlayConfetti(confettiPositions[i]);
            }
        }        
    }

    private void OnEnable()
    {
        EventManager.OnLevelSuccess.AddListener(()=> canSwitch = false);
    }

    private void OnDisable()
    {
        EventManager.OnLevelSuccess.RemoveListener(() => canSwitch = false);
    }

    public void StartHighlight()
    {        
        StartCoroutine(StartHighlightCo());
    }

    private IEnumerator StartHighlightCo() 
    {
        while (canSwitch)
        {            
            MeshRenderer.material.DOColor(highlightedMaterial.color, 0.2f);
            yield return new WaitForSeconds(0.25f);
            MeshRenderer.material.DOColor(defaultMaterial.color, 0.2f);            
            yield return new WaitForSeconds(0.25f);
        }             
    }

}
