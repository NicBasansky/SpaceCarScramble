using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car.Combat;

namespace Car.Pickup
{
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "PowerUp", menuName = "CarMadness/New PowerUp", order = 0)]
    public class PowerUp : ScriptableObject 
    {
        [SerializeField] Pickup pickupPrefab;
        [SerializeField] Weapon weapon;
        [SerializeField] int ammoAmount;
        [SerializeField] bool isShield = false;
        [SerializeField] float duration;


        public Pickup Spawn(Vector3 position)
        {
            Pickup pickup = Instantiate(pickupPrefab);
            pickup.transform.position = position;
            pickup.Setup(this);
            return pickup;
        }

        public bool GetIsShield()
        {
            return isShield;
        }

        public Weapon GetWeapon()
        {
            return weapon;
        }

        public int GetAmmoContribution()
        {
            return ammoAmount;
        }

        public float GetDurationContribution()
        {
            return duration;
        }

        

    }

}