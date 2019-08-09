using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem))]
public class CoinParticle : MonoBehaviour
{

    public UnityEvent onSpawn;
    public UnityEvent onCollide;
    public UnityEvent onStop;
    
    ParticleSystem particles;
    ParticleSystem.EmissionModule emission;

    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
        emission = particles.emission;
    }

    public void Play(int e = -1)
    {
        if (e > 0f)
        {
            emission.rateOverTime = e + 0.1f;
        }
        particles.Stop();
        particles.Play();
        StopAllCoroutines();
        StartCoroutine(Emitting((int)e));
    }

    private void OnParticleCollision(GameObject other)
    {
        onCollide.Invoke();
    }

    IEnumerator Emitting(int e)
    {
        int count = 0;
        while (particles.isEmitting)
        {
            while (count < particles.particleCount)
            {
                onSpawn.Invoke();
                count++;
            }
            count = particles.particleCount;
            yield return null;
        }
        onStop.Invoke();
    }

}
