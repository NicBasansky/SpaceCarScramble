using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car.Pickup;
using System;




// polish (pickups, pickup placement, balance)
// sound

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
        [SerializeField] GameObject shield;
        [SerializeField] float maxShieldLife = 5f;
        [SerializeField] float shieldLife = 3f;
        [SerializeField] Transform fxParent;
        [SerializeField] AudioClip weaponSwitchSound;
        bool shieldUp = false;
        int numSlots = 3;
        WeaponSlot activeWeaponSlot = null;
        AudioSource audioSource;
        int slotIndex = 0;
        int hasWeaponInSlot = 0;

        bool cooldown = false;
        bool infiniteAmmo = false;

        Transform weaponLaunchTransform;
        List<Cannon> spawnedCannons = new List<Cannon>();

        public delegate void CycleWeaponsDelegate(int toSlotIndex);
        public CycleWeaponsDelegate cycleWeapons;

        public delegate void UpdateWeaponIconDelegate(Sprite newIcon, int slot);
        public UpdateWeaponIconDelegate updateWeaponIcon;

        public delegate void UpdateAmmoDelegate(WeaponSlot slot, int newAmmo);
        public UpdateAmmoDelegate updateAmmo;


        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        void Start()
        {
            BuildWeaponSlots();
            Setup(defaultWeapon);
            activeWeaponSlot = weaponSlots[0];
            cooldown = true;
            StartCoroutine(Cooldown());
            EnableShield(false);
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

                if (newWeapon.GetWeaponIcon() != null)
                {
                    updateWeaponIcon(newWeapon.GetWeaponIcon(), slot);  
                    updateAmmo(weaponSlots[slot], newWeapon.GetStartingAmmo());

                }
            }       
        }

        public void HandleNewPowerUp(PowerUp powerUp)
        {
            if (powerUp.GetIsShield())
            {
                AffectShieldLife(powerUp.GetDurationContribution());
                return; // ??
            }

            if (DoesPlayerHaveWeapon(powerUp.GetWeapon()))
            {
                // to which slot?
                foreach(WeaponSlot slot in weaponSlots)
                {
                    if (slot.assignedSlot == hasWeaponInSlot)
                    {
                        AffectAmmo(slot, powerUp.GetAmmoContribution()); 
                    }   // TODO increase power
                }
            }
            else
            {
                AddWeapon(powerUp.GetWeapon());
            }
        }

        public void AffectShieldLife(float delta)
        {
            shieldLife += delta;
            shieldLife = Mathf.Clamp(shieldLife, 0, maxShieldLife);
        }

        public bool GetIsShieldUp()
        {
            return shieldUp;
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

        public void EnableShield(bool enabled)
        {
            if (shield == null) return;

            if (shieldLife <= 0)
            {
                shield.SetActive(false);
                shieldUp = false;
                return;
            }
            
            shield.SetActive(enabled);
            if (enabled)
            {
                shieldUp = true;
            }
            else
            {
                shieldUp = false;
            }
            
        }

        public float GetShieldLife()
        {
            return shieldLife;
        }

        public void CycleWeapon()
        {
            Weapon currentWeapon = weaponSlots[slotIndex].weapon;

            slotIndex++;
            slotIndex %= weaponSlots.Count;

            if (weaponSlots[slotIndex].weapon != null)
            {
                activeWeaponSlot = weaponSlots[slotIndex];
                cycleWeapons(slotIndex);
                AudioSource.PlayClipAtPoint(weaponSwitchSound, transform.position);
                return;
            }
            
            int counter = 0;
            while (weaponSlots[slotIndex].weapon == null)
            {
                slotIndex++;
                slotIndex %= weaponSlots.Count;
                counter++;
                if (counter >= numSlots)
                {
                    slotIndex = 0;
                    break;
                }
                               
            }  
            
            if (!ReferenceEquals(currentWeapon, weaponSlots[slotIndex].weapon))
            {
                AudioSource.PlayClipAtPoint(weaponSwitchSound, transform.position);
            }
            

            activeWeaponSlot = weaponSlots[slotIndex];

            cycleWeapons(slotIndex);
        }

        public void AffectAmmo(WeaponSlot slot, int delta)
        {
            if (slot.weapon.GetHasInfiniteAmmo()) return;

            slot.ammo += delta;
            slot.ammo = Mathf.Clamp(slot.ammo, 0, slot.weapon.GetMaxAmmo());
            updateAmmo(slot, slot.ammo);
            
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
                if (activeWeaponSlot.weapon == null) return;          
                // TODO Get projectiles from object pool
                Projectile projectile = Instantiate(activeWeaponSlot.weapon.GetProjectile(), activeWeaponSlot.launchTransform.position, Quaternion.identity);
        
                projectile.SetupProjectile(activeWeaponSlot.launchTransform, activeWeaponSlot.weapon, this.gameObject, fxParent);                
                if (this.gameObject.tag == "Player")
                {
                    AffectAmmo(activeWeaponSlot, -1);
                    if (activeWeaponSlot.ammo <= 0 && !activeWeaponSlot.weapon.GetHasInfiniteAmmo())
                    {
                        activeWeaponSlot.weapon = null;
                        CycleWeapon();
                    }
                }

                if (activeWeaponSlot.weapon.GetFireSound())
                {
                    AudioSource.PlayClipAtPoint(activeWeaponSlot.weapon.GetFireSound(), transform.position);
                }
                
        } 

        public float GetShieldLifeFraction()
        {
            if (maxShieldLife == 0)
            {
                return 0;
            }
            return shieldLife / maxShieldLife;
        }

        
        public Weapon GetDefaultWeapon()
        {
            return defaultWeapon;
        }

        public Weapon GetCurrentWeapon()
        {
            return activeWeaponSlot.weapon;
        }
    }

}
