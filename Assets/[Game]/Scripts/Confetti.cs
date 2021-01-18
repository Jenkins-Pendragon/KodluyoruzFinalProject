using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    public ParticleSystem[] particles;
    public GameObject ragdollParent;
    public static Confetti Instance;

    public void Awake()
    {
        Instance = this;
    }

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

    public void PlayConfetti(Transform transform) 
    {        
        for (int i = 0; i < particles.Length; i++)
        {
            ParticleSystem particle = Instantiate(particles[i], transform.position, Quaternion.identity);
            particle.Play();
        }
    }

        
}
