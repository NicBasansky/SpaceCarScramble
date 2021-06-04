using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car.Combat
{
    public class Grenade : Projectile
    {
       
        CarController player;
        
        // Start is called before the first frame update
        void Start()
        {
            player = instigator.GetComponent<CarController>();
            player.setIsWaitingOnDetonation(true);
            
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

      
    }
}
