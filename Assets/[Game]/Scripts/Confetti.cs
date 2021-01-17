using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    public ParticleSystem[] particles;
    public GameObject ragdollParent;

    private void OnEnable()
    {
        EventManager.OnLevelSuccess.AddListener(PlayParticle);
    }

    private void OnDisable()
    {
        EventManager.OnLevelSuccess.RemoveListener(PlayParticle);
    }
    private void PlayParticle()
    {
        transform.position = ragdollParent.transform.position;
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
    }

        
}
