using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Car.Combat;

public class ShieldDurabilityUI : MonoBehaviour
{
    [SerializeField] RectTransform durabilityFill;
    [SerializeField] Fighter fighter;
    

    void Update()
    {
        if (fighter == null) return;

        durabilityFill.localScale = new Vector3(fighter.GetShieldLifeFraction(), 1, 1);
    }
    
}
