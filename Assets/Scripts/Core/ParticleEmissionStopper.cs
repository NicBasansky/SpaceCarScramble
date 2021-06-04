using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEmissionStopper : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particleSystems;
    
    public void StopAllParticleEmission()
    {
        foreach(var ps in particleSystems)
        {
            var em = ps.emission;
            em.enabled = false;
        }
    }

    public void StartAllParticleEmission()
    {
        foreach(var ps in particleSystems)
        {
            var em = ps.emission;
            em.enabled = true;
        }
    }

    public bool IsEmitting()
    {
        return particleSystems[0].isEmitting;
    }
}
