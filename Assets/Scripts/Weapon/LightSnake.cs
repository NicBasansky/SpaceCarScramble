using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car.Combat
{
    public class LightSnake : Projectile
    {
        [SerializeField] float sensitivity = 1f;
        CarController player;

        void Start()
        {
            player = instigator.GetComponent<CarController>();
        }

        protected override void MoveForward()
        {
            rb.AddForce(launchTransform.forward * weapon.GetProjectileSpeed() * Time.deltaTime);      
           
        }

        protected override void FixedUpdate()
        {
            if (!Mathf.Approximately(player.GetTurnInput(), 0))
            {
                rb.AddForce(new Vector3(-player.GetTurnInput() * sensitivity, 0, 0));  
            }
             

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
    }

}
