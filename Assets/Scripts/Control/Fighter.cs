using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car.Pickup;
using System;


// need to handle shield
// need to handle ammo
namespace Car.Combat
{
    [System.Serializable]
    public class WeaponSlot
    {
        public int assignedSlot = 0;
        public Weapon weapon;
        public int ammo = 0;    
        public Transform launchTransform;
    }

    public class Fighter : MonoBehaviour
    {
        [SerializeField] Weapon defaultWeapon;
        [SerializeField] List<WeaponSlot> weaponSlots = new List<WeaponSlot>(); // more weapon slots?
        [SerializeField] Transform weaponTransform;
        [SerializeField] float attackAngle = 30f;
        [SerializeField] int ammo = 0;
        int numSlots = 3;
        WeaponSlot activeWeaponSlot = null;
        int slotIndex = 0;
        int hasWeaponInSlot = 0;

        bool cooldown = false;
        bool infiniteAmmo = false;

        Transform weaponLaunchTransform;
        List<Cannon> spawnedCannons = new List<Cannon>();


        void Start()
        {
            BuildWeaponSlots();
            Setup(defaultWeapon);
            activeWeaponSlot = weaponSlots[0];
            cooldown = true;
            StartCoroutine(Cooldown());
        }

        private void BuildWeaponSlots()
        {
            for (int i = 0; i < numSlots; i++)
            {
                var newSlot = new WeaponSlot();
                newSlot.assignedSlot = i;
                weaponSlots.Add(newSlot);
            }
        }

        public void Setup(Weapon newWeapon)
        {       
            int slot = FindFirstEmptySlot();
            if (slot < 0)
            {
                slot = activeWeaponSlot.assignedSlot;
            }
            if (weaponTransform != null)
            {
                weaponSlots[slot].weapon = newWeapon;
                weaponSlots[slot].ammo = newWeapon.GetStartingAmmo();

                GameObject spawned = Instantiate(newWeapon.GetPrefab(), weaponTransform);
                Cannon spawnedCannon = spawned.GetComponent<Cannon>();
                spawnedCannons.Add(spawnedCannon);

                weaponSlots[slot].launchTransform = spawnedCannon.GetLaunchTransform();
                spawnedCannon.slotId = slot;         
            }       
        }

        public void HandleNewPowerUp(PowerUp powerUp)
        {
            if (powerUp.GetIsShield())
            {
               // SetupShield();
                return; // ??
            }

            if (DoesPlayerHaveWeapon(powerUp.GetWeapon()))
            {
                // to which slot?
                foreach(WeaponSlot slot in weaponSlots)
                {
                    if (slot.assignedSlot == hasWeaponInSlot)
                    {
                        AddAmmo(slot, powerUp.GetAmmoContribution()); 
                    }   // TODO increase power
                }
            }
            else
            {
                AddWeapon(powerUp.GetWeapon());
            }
        }

        private bool DoesPlayerHaveWeapon(Weapon newWeapon)
        {
            foreach (Weapon w in GetAllWeapons())
            {
                if (ReferenceEquals(newWeapon, w))
                {
                    foreach (WeaponSlot slot in weaponSlots)
                    {
                        if (ReferenceEquals(slot.weapon, w))
                        {
                            hasWeaponInSlot = slot.assignedSlot;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public int FindFirstEmptySlot()
        {
            for (int i = 0; i < weaponSlots.Count; i++)
            {
                if (weaponSlots[i].weapon != null)
                {
                    continue;
                }
                else
                {
                    return i;
                }
            }
            return -1; // all full
            
        }

        public void Attack(Transform target)
        {
            Vector3 direction = new Vector3(target.position.x - transform.position.x,
                                            transform.position.y,
                                            target.position.z - transform.position.z);
            if (Vector3.Angle(transform.forward, direction) < attackAngle
                            && !cooldown)
            {
                FireWeapon();

                cooldown = true;
                StartCoroutine(Cooldown());
            }
        }

        public void CycleWeapon()
        {
            slotIndex++;
            slotIndex %= weaponSlots.Count;

            if (weaponSlots[slotIndex].weapon != null)
            {
                activeWeaponSlot = weaponSlots[slotIndex];
                return;
            }
            
            while (weaponSlots[slotIndex].weapon == null)
            {
                slotIndex++;
                slotIndex %= weaponSlots.Count;
            }          
            
            activeWeaponSlot = weaponSlots[slotIndex];

        }

        public void AddAmmo(WeaponSlot slot, int additionalAmmo)
        {
            slot.ammo += additionalAmmo;
            
        }

        public List<Weapon> GetAllWeapons()
        {
            List<Weapon> weapons = new List<Weapon>();
            foreach(WeaponSlot w in weaponSlots)
            {
                weapons.Add(w.weapon);
            }
            return weapons;
        }

        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(activeWeaponSlot.weapon.GetAttackCooldownSeconds());
            cooldown = false;
        }

        public void AddWeapon(Weapon newWeapon)
        {
            if (FindFirstEmptySlot() < 0) // slots full
            {
                SwapWeapons(newWeapon);
                return;
            }
            Setup(newWeapon);
        }

        private void SwapWeapons(Weapon newWeapon)
        {
            Cannon removedWeapon = null;
            foreach(Cannon cannon in spawnedCannons)
            {
                if (activeWeaponSlot.assignedSlot == cannon.slotId)
                {
                    removedWeapon = cannon;
                }
            }

           spawnedCannons.Remove(removedWeapon);
           Destroy(removedWeapon.gameObject, 0.2f);

           Setup(newWeapon);

        }

        public void FireWeapon()
        {
            // TODO Get projectiles from object pool
            Projectile projectile = Instantiate(activeWeaponSlot.weapon.GetProjectile(), activeWeaponSlot.launchTransform.position, Quaternion.identity);
            
            projectile.SetupProjectile(activeWeaponSlot.launchTransform, activeWeaponSlot.weapon, this.gameObject);
            
        }
    }

}