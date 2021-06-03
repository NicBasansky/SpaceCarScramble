using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car.Pickup
{
    public class PickupSpawner : MonoBehaviour
    {
        [SerializeField] PowerUp pickupToSpawn;
        float respawnDelay = 20f;
        Pickup spawned = null;
        bool hasPickedUp = false;

        public event Action pickupRespawn;
        
        void Awake()
        {
            SpawnPickup();
        }

        private void SpawnPickup()
        {
            spawned = pickupToSpawn.Spawn(transform.position);
            spawned.transform.SetParent(transform);
            spawned.onPickedUp += OnPickedUp;
        }

        private void OnPickedUp()
        {

            // if (spawned != null)
            // {
            //     spawned.onPickedUp -= OnPickedUp;
            // }
            spawned.gameObject.SetActive(false);
            StartCoroutine(Respawn());
            
        }

        IEnumerator Respawn()
        {
            if (!hasPickedUp)
            {
                hasPickedUp = true;
            
                yield return new WaitForSeconds(respawnDelay);

                spawned.gameObject.SetActive(true);
                hasPickedUp = false;
            }
        }
    }
}
