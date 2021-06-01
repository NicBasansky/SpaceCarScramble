using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Car.Combat;

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
        int slotIndex = 0;

        Fighter fighter;
    

        void Awake()
        {
            fighter = GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>();
            fighter.cycleWeapons += CycleIcons;
            fighter.updateWeaponIcon += AddWeaponIcon;
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

            slotIcon2.gameObject.SetActive(false);
            emptyImage2.gameObject.SetActive(true);

            slotIcon3.gameObject.SetActive(false);
            emptyImage3.gameObject.SetActive(true);
            
            slotSelection1.gameObject.SetActive(true);
            slotSelection2.gameObject.SetActive(false);
            slotSelection3.gameObject.SetActive(false);
        }

        public void AddWeaponIcon(Sprite newIcon, int slot)
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

        
    }

}