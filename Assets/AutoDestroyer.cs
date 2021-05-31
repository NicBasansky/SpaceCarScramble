using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
    [SerializeField] float destroyDelay = 5f;

    
    void Start()
    {
        Destroy(this.gameObject, destroyDelay);
    }

}
