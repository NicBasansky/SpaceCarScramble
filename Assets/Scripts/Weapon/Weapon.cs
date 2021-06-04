using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car.Combat
{    
    [CreateAssetMenu(fileName = "Weapon", menuName = "CarMadness/Weapon", order = 0)]
    public class Weapon : ScriptableObject 
    {
        
        [SerializeField] GameObject prefab;
        [SerializeField] Sprite icon;
        [Tooltip("If false, will raycast rather than lob a projectile")]
        [SerializeField] bool isProjectile = false;
        [SerializeField] int startingAmmo = 0;
        [SerializeField] int maxAmmo = 15;
        [SerializeField] bool infiniteAmmo = false;
        [SerializeField] Projectile projectilePrefab;
        [SerializeField] GameObject impactFX = null;
        [SerializeField] float projectileSpeed = 400000f;
        [SerializeField] bool shouldStopOnImpact = true;
        [SerializeField] bool shouldExplode = true;
        [SerializeField] float explosionForce = 2500f;
        [SerializeField] float explosionRadius = 5f;
        //[Tooltip("A small amount of force applied when the projectile hits an enemy. Should be a small amount as they will explode off scene with explosion force when they die")]
        [SerializeField] float hitForce = 80000f;
        [SerializeField] float damage = 10f;
        [SerializeField] float attackCooldownSeconds = 2f;
        [SerializeField] AudioClip fireSound;
        [SerializeField] AudioClip impactSound;

        //[SerializeField] 
        // particle systems

        public GameObject GetPrefab()
        {
            return prefab;
        }

        public Sprite GetWeaponIcon()
        {
            return icon;
        }

        public GameObject GetImpactFX()
        {
            return impactFX;
        }

        public bool GetHasInfiniteAmmo()
        {
            return infiniteAmmo;
        }

        public Projectile GetProjectile()
        {
            return projectilePrefab;
        }

        public int GetStartingAmmo()
        {
            return startingAmmo;
        }

        public int GetMaxAmmo()
        {
            return maxAmmo;
        }

        public float GetProjectileSpeed()
        {
            return projectileSpeed;
        }

        public float GetExplosionForce()
        {
            return explosionForce;
        }

        public float GetHitForce()
        {
            return hitForce;
        }

        public float GetExplosionRadius()
        {
            return explosionRadius;
        }

        public float GetDamage()
        {
            return damage;
        }

        public bool GetShouldProjectileStopOnImpact()
        {
            return shouldStopOnImpact;
        }

        public bool GetShouldExplode()
        {
            return shouldExplode;
        }

        public float GetAttackCooldownSeconds()
        {
            return attackCooldownSeconds;
        }

        public AudioClip GetImpactSound()
        {
            return impactSound;
        }

        public AudioClip GetFireSound()
        {
            return fireSound;
        }

    }
    

}