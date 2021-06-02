using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car.Combat;

public class ParticleCollisionDetector : MonoBehaviour
{

    [SerializeField] Projectile projectile;
    
    void OnParticleCollision(GameObject other)
    {
        Collider collider = other.GetComponent<Collider>();
        if (collider == null) return;

        projectile.OnTriggerEnter(collider);
    }
}
