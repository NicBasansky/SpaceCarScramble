using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarsLeftUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI numCarsLeftText;
    AiCarManager carManager;

    void Awake()
    {
        carManager = FindObjectOfType<AiCarManager>();
        carManager.onUIUpdate += UpdateUI;
    }

    private void UpdateUI()
    {
        numCarsLeftText.text = carManager.GetNumRemaining().ToString();
    }
}
