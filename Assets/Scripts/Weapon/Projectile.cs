using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car.Core;
using Car.Control;

namespace Car.Combat
{
    public abstract class Projectile : MonoBehaviour
    {
        protected Rigidbody rb;
        protected Weapon weapon;
        [SerializeField] protected ParticleEmissionStopper emissionStopper;
        [SerializeField] protected float lifeTime = 4f;
        [SerializeField] protected float fadeTimeBeforeDestroy = 2f;
        protected bool shouldExplode = false;
        protected bool shouldStopOnImpact = true;
        protected bool isLaunching = false;
        protected bool isExploding = false;
        protected bool simpleProjectileHit = false;
        protected bool shieldUp = false;
        protected Transform launchTransform;
        //Collision target = null;
        protected Collider target = null;
        protected GameObject instigator = null;
        protected Transform fxParent;
        //protected AudioSource audioSource;

        void Awake()
        {
            rb = GetComponent<Rigidbody>(); 
            //audioSource = GetComponent<AudioSource>();
        }   

        void Start()
        {
            Invoke("StopParticleEmissions", lifeTime);
            Destroy(this.gameObject, lifeTime + 3f);
        }

        protected virtual void FixedUpdate()
        {
            if (isLaunching)
            {
                MoveForward();
                isLaunching = false;

            }

            
            if (isExploding)
            {
                Explode();
                shouldExplode = false;
            }

            if (simpleProjectileHit)
            {
                SimpleProjectileHit();
                simpleProjectileHit = false;
            }
        } 

        protected virtual void MoveForward()
        {
            rb.AddForce(launchTransform.forward * weapon.GetProjectileSpeed() * Time.deltaTime);
        }

        protected void Explode()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, weapon.GetExplosionRadius());
                foreach (Collider hit in hits)
                {
                    if (hit.gameObject.tag == "Car")
                    {
                        AIController aiController = hit.GetComponent<AIController>();
                        if (aiController == null) return;
                      
                        //Rigidbody hitRB = aiController.GetBodyRigidBody();
                        //aiController.FreezeMovementFromExplosion(3f);
                        //hitRB.AddExplosionForce(weapon.GetExplosionForce(), transform.position, weapon.GetExplosionRadius(), 1, ForceMode.Impulse);                                                         
                    
                        //Simple Proj Hit
                        Rigidbody hitRB = aiController.GetSphereRigidBody();
                        aiController.FreezeMovementFromHit(0.5f);
                        if (target != null)
                        {
                            Vector3 forceDirection = transform.position - target.transform.position;
                            hitRB.AddForce(forceDirection.normalized * weapon.GetHitForce());
                        }
                       

                        aiController.AffectHealth(weapon.GetDamage());       
                    
                    }
                }
        }

        protected void SimpleProjectileHit()
        {
            if (target == null) return;
            if (target.gameObject.tag == "Car")
            {
                AIController aiController = target.gameObject.GetComponent<AIController>();
                if (aiController == null) return;

                aiController.AffectHealth(weapon.GetDamage());                      
                      
                Rigidbody hitRB = aiController.GetSphereRigidBody();
                aiController.FreezeMovementFromHit(0.5f);
                Vector3 forceDirection = transform.position - target.transform.position;
                hitRB.AddForce(forceDirection.normalized * weapon.GetHitForce());
                
            }
            


        }

        protected void PlayImpactFX()
        {
            if (weapon == null)
            {
                print("weapon is null");
                return;
            }
            GameObject impactFx = weapon.GetImpactFX();
            if (impactFx != null)
            {

                GameObject fx = Instantiate(impactFx, transform.position, Quaternion.identity);
                fx.transform.parent = fxParent;
                
            }
            PlayImpactSound();
        }

        
        protected void PlayImpactFX(Vector3 location)
        {
            GameObject impactFx = weapon.GetImpactFX();
            if (impactFx != null)
            {

                GameObject fx = Instantiate(impactFx, location, Quaternion.identity);
                fx.transform.parent = fxParent;

            }
            PlayImpactSound();
        }
        
        protected void PlayImpactSound()
        {
            AudioClip impactSound = weapon.GetImpactSound();
            if (impactSound != null)
            {
                if (weapon.GetProjectile() as Grenade)
                {
                    AudioSource.PlayClipAtPoint(impactSound, instigator.transform.position);

                }
                else
                {
                    AudioSource.PlayClipAtPoint(impactSound, transform.position);

                }
            }

            
        }

        public void OnTriggerEnter(Collider other) // TODO bouncy projectiles?
        {        
            if (other.gameObject == instigator) return;
            
            if (other.gameObject.tag == "Shield")
            {
                Vector3 vecPlayerToProjectile = (transform.position - other.transform.position);
                Vector3 impactPos = vecPlayerToProjectile.normalized * 3f;

                PlayImpactFX(other.transform.position + impactPos);
                //DisableCollider();
                //StopEmissionsAndDestroy(3f);
                print("hit shield");
                Destroy(this.gameObject);
                return;
          
            }  

            if (other.gameObject.tag == "Car")
            {         
                target = other;     
                if (shouldExplode)
                {           
                    isExploding = true;
                }
                else
                {                   
                    simpleProjectileHit = true;
                }
                PlayImpactFX();
            }  
            else if (other.gameObject.tag == "Player" && instigator.tag != "Player")
            {
                print("hit player");
                Health playerHealth = other.gameObject.GetComponent<Health>();
                if (!playerHealth.GetIsShieldUp())
                {
                    playerHealth.AffectHealth(-weapon.GetDamage());
                    PlayImpactFX();
                }
            }  
            else
            {
                if (shouldStopOnImpact)
                {
                    PlayImpactFX();
                    DisableCollider();
                    StopEmissionsAndDestroy(3f);
                    
                    return;
                }
            }

            if (shouldStopOnImpact && other.gameObject.layer != LayerMask.NameToLayer("Ground Layer"))
            {
                DisableCollider();
                StopEmissionsAndDestroy(3f);
                return;
            }
            
            
            Invoke("StopParticleEmmissions", lifeTime - fadeTimeBeforeDestroy);
            Invoke("DisableCollider", lifeTime - fadeTimeBeforeDestroy);
            
             

        }

        protected void StopParticleEmissions()
        {
            if (emissionStopper != null)
            {
                emissionStopper.StopAllParticleEmission();

            }
        }

        protected void DisableCollider()
        {
            Collider collider = GetComponent<Collider>();
            if (collider)
            {
                collider.enabled = false;
            }
        }

        protected void StopEmissionsAndDestroy(float destroyDelay)
        {
            if (emissionStopper != null)
            {
                emissionStopper.StopAllParticleEmission();

            }                                                 
            Destroy(this.gameObject, destroyDelay); // TODO remove via object poo            
        }

        public virtual void SetupProjectile(Transform launchTransform, Weapon weapon, GameObject instigator, Transform fxParent)
        {
            this.weapon = weapon;
            this.launchTransform = launchTransform;
            this.instigator = instigator;
            this.fxParent = fxParent;
            shouldExplode = weapon.GetShouldExplode();
            shouldStopOnImpact = weapon.GetShouldProjectileStopOnImpact();
            isLaunching = true;
            transform.parent = this.fxParent;
        }

        // void OnDrawGizmosSelected()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(transform.position, weapon.GetExplosionRadius());
        // }

    }
}
