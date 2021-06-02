using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Car.Combat;
using TMPro;

namespace Car.UI
{
    public class WeaponIconsUI : MonoBehaviour
    {
        [SerializeField] Image slotIcon1;       
        [SerializeField] Image slotIcon2;      
        [SerializeField] Image slotIcon3;
        [SerializeField] Image slotSelection1;
        [SerializeField] Image slotSelection2;
        [SerializeField] Image slotSelection3;
        [SerializeField] Image emptyImage1;
        [SerializeField] Image emptyImage2;
        [SerializeField] Image emptyImage3;
        [SerializeField] GameObject ammoGO1;
        [SerializeField] GameObject ammoGO2;
        [SerializeField] GameObject ammoGO3;
        [SerializeField] TextMeshProUGUI ammoCount1;
        [SerializeField] TextMeshProUGUI ammoCount2;
        [SerializeField] TextMeshProUGUI ammoCount3;

        int slotIndex = 0;

        Fighter fighter;
    

        void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
            fighter.cycleWeapons += CycleIcons;
            fighter.updateWeaponIcon += AddWeaponIcon;
            fighter.updateAmmo += UpdateAmmo;
        }

        void Start()
        {
            ResetIcons();
        }

        private void CycleIcons(int slotIndex)
        {
            print("slotIndex: " + slotIndex);
            switch (slotIndex)
            {
                case 0:
                    slotSelection1.gameObject.SetActive(true);
                    slotSelection2.gameObject.SetActive(false);
                    slotSelection3.gameObject.SetActive(false);
                    break;
                case 1:
                    slotSelection1.gameObject.SetActive(false);
                    slotSelection2.gameObject.SetActive(true);
                    slotSelection3.gameObject.SetActive(false);
                    break;
                case 2:
                    slotSelection1.gameObject.SetActive(false);
                    slotSelection2.gameObject.SetActive(false);
                    slotSelection3.gameObject.SetActive(true);
                    break;
            }
            
        }

        private void ResetIcons()
        {
            Weapon defaultWeapon = fighter.GetDefaultWeapon();
            Sprite icon = defaultWeapon.GetWeaponIcon();
            if (icon != null)
            {
                slotIcon1.sprite = icon;
            }

            emptyImage1.gameObject.SetActive(false);
            ammoGO1.SetActive(false);

            slotIcon2.gameObject.SetActive(false);
            emptyImage2.gameObject.SetActive(true);
            ammoGO2.SetActive(false);

            slotIcon3.gameObject.SetActive(false);
            emptyImage3.gameObject.SetActive(true);
            ammoGO3.SetActive(false);
            
            slotSelection1.gameObject.SetActive(true);
            slotSelection2.gameObject.SetActive(false);
            slotSelection3.gameObject.SetActive(false);
        }

        private void AddWeaponIcon(Sprite newIcon, int slot)
        {
            switch (slot)
            {
                case 0:
                    slotIcon1.sprite = newIcon;
                    slotIcon1.gameObject.SetActive(true);
                    emptyImage1.gameObject.SetActive(false);
                    break;
                case 1:
                    slotIcon2.sprite = newIcon;
                    slotIcon2.gameObject.SetActive(true);
                    emptyImage2.gameObject.SetActive(false);
                    break;
                case 2:
                    slotIcon3.sprite = newIcon;
                    slotIcon3.gameObject.SetActive(true);
                    emptyImage3.gameObject.SetActive(false);
                    break;
            }
        }

        private void UpdateAmmo(WeaponSlot slot, int newAmmo)
        {
            bool infiniteAmmo = slot.weapon.GetHasInfiniteAmmo();
            switch (slot.assignedSlot)
            {
                case 0:
                    if (infiniteAmmo)
                    {
                        ammoGO1.SetActive(false);
                        return;
                    }
                    else 
                    {
                        ammoGO1.SetActive(true);
                        ammoCount1.text = newAmmo.ToString();
                        if (newAmmo <= 0)
                        {
                            RemoveIcon(0);
                        }
                    }
                    break;
                case 1:
                    if (infiniteAmmo)
                    {
                        ammoGO2.SetActive(false);
                        return;
                    }
                    else 
                    {
                        ammoGO2.SetActive(true);
                        ammoCount2.text = newAmmo.ToString();
                        if (newAmmo <= 0)
                        {
                            RemoveIcon(1);
                        }
                    }
                    break;
                case 2:
                    if (infiniteAmmo)
                    {
                        ammoGO3.SetActive(false);
                        return;
                    }
                    else 
                    {
                        ammoGO3.SetActive(true);
                        ammoCount3.text = newAmmo.ToString();
                        if (newAmmo <= 0)
                        {
                            RemoveIcon(2);
                        }
                    }
                    break;
            }
        }

        void RemoveIcon(int slot) // TODO test if null e
        {
            switch (slot)
            {
                case 0:
                    slotIcon1.gameObject.SetActive(false);
                    ammoGO1.SetActive(false);
                    emptyImage1.gameObject.SetActive(false);
                    break;
                case 1:
                    slotIcon2.gameObject.SetActive(false);
                    ammoGO2.SetActive(false);
                    emptyImage2.gameObject.SetActive(true);
                    break;
                case 2:
                    slotIcon3.gameObject.SetActive(false);
                    ammoGO3.SetActive(false);
                    emptyImage3.gameObject.SetActive(true);
                    break;
            }
        }
        
    }

}