using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car.Combat
{
    public class RainbowProjectile : Projectile
    {
        // bool alreadyHit = false;
        // float slowAmount = 5f;
        // float slowDelay = 3f;

        // public override void OnTriggerEnter(Collider other)
        // {
        //     if (other.gameObject.tag == ("Car"))// || (other.gameObject.tag == ("Player") && instigator.gameObject.tag != ("Player")))
        //     {
        //         Rigidbody rb = other.GetComponent<Rigidbody>();
        //         if (rb)
        //         {
        //             StartCoroutine(SlowTarget(rb));
        //         }
        //     }
        //     base.OnTriggerEnter(other);
        // }

        // IEnumerator SlowTarget(Rigidbody rb)
        // {
        //     if(!alreadyHit)
        //     {
        //         alreadyHit = true;
        //         rb.velocity -= new Vector3(0, 0, slowAmount);

        //         yield return new WaitForSeconds(slowDelay);

        //         rb.velocity += new Vector3(0, 0, slowAmount);
        //         alreadyHit = false;
        //     }
            
        // }

    }

}