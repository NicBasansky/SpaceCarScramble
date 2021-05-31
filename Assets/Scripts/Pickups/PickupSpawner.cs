using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car.Pickup
{
    public class PickupSpawner : MonoBehaviour
    {
        [SerializeField] PowerUp pickupToSpawn;
        Pickup spawned = null;
        
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
            // GetComponentInChildren<Collider>().enabled = false;
            // GetComponentInChildren<MeshRenderer>().enabled = false;

            if (spawned != null)
            {
                spawned.onPickedUp -= OnPickedUp;
            }
            spawned.gameObject.SetActive(false);
        }
    }
}
