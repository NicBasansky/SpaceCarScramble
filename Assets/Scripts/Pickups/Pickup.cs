using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car.Combat;

namespace Car.Pickup
{
    public class Pickup : MonoBehaviour
    {
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
            }    
        }

        private void PickupPowerUp(GameObject player)
        {           
            player.GetComponent<Fighter>().HandleNewPowerUp(powerUp);
        }
  
    }

}