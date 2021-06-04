using System.Collections;
using System.Collections.Generic;
using Car.Core;
using UnityEngine;
using System;

public class AiCarManager : MonoBehaviour
{
    //List<Health> remainingCars = new List<Health>();

    public event Action onUIUpdate;
    public event Action endMatch; 

    int numRemaining = 0;

    void Start()
    {
        foreach(Transform car in transform)
        {
            Health health = car.GetComponent<Health>();
            if (health == null) return;

            // remainingCars.Add(health);
            health.onAiCarDied += OnCarDied;
        }
        numRemaining = GetStartingNumAiCars();
        onUIUpdate();
    }

    public int GetNumRemaining()
    {
        return numRemaining;
    }
    
    public int GetStartingNumAiCars()
    {
        int numCars = 0;
        foreach(Transform car in transform)
        {
            numCars++;
        }
        return numCars;
    }

    private void OnCarDied()
    {
        print("car died");
        numRemaining--;
        if (numRemaining == 0)
        {
            endMatch();
        }
        onUIUpdate();
    }
}
