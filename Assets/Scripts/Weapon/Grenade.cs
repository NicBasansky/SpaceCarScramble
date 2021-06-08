using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car.Combat
{
    public class Grenade : Projectile
    {
       
        CarController player;
        [SerializeField] float lifetime = 2.5f;
        float timeSinceFiring = Mathf.Infinity;
        
        // Start is called before the first frame update
        void Start()
        {
            player = instigator.GetComponent<CarController>();
            player.setIsWaitingOnDetonation(true);
            timeSinceFiring = 0;
            
        }

        
        void Update()
        {
            if (!isExploding && Input.GetKeyDown(KeyCode.Space))
            {          
                isExploding = true;
                DisableCollider();
                PlayImpactFX(transform.position);
                player.setIsWaitingOnDetonation(false);
                StopEmissionsAndDestroy(.2f);

            }
            
            timeSinceFiring += Time.deltaTime;
            if (timeSinceFiring >= lifeTime)
            {
                player.setIsWaitingOnDetonation(false);
            }

        }

        protected override void FixedUpdate()
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
                player.setIsWaitingOnDetonation(false);
            }

            if (simpleProjectileHit)
            {
                SimpleProjectileHit();
                simpleProjectileHit = false;
                player.setIsWaitingOnDetonation(false);
            }
        }

        private void OnDestroy() 
        {
            player.setIsWaitingOnDetonation(false);
        }

      
    }
}
