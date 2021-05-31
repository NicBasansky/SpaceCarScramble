using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car.Combat;

namespace Car.Pickup
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] ParticleEmissionStopper emissionStopper;
        PowerUp powerUp;
        Fighter fighter;

        public event Action onPickedUp;

        public void Setup(PowerUp details)
        {
            powerUp = details;
        }
        
        private void OnTriggerEnter(Collider other) 
        {
            if (other.gameObject.tag == "Player")
            {
               PickupPowerUp(other.gameObject);
               onPickedUp();
               if (emissionStopper != null)
               {
                    emissionStopper.StopAllParticleEmission();

               }
            }    
        }

        private void PickupPowerUp(GameObject player)
        {           
            player.GetComponent<Fighter>().HandleNewPowerUp(powerUp);
        }
  
    }

}