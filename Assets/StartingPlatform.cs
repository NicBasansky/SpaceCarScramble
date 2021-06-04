using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPlatform : MonoBehaviour
{
    [SerializeField] float startSoundDelay = 1.5f;
    [SerializeField] AudioClip startSound;
    [SerializeField] float colliderDelay = 3f;
    [SerializeField] float goDelay = 1f;

    void Start()
    {
        gameObject.SetActive(true);
        StartCoroutine(PlayStartSound());
        StartCoroutine(Dissolve());
    }

    IEnumerator PlayStartSound()
    {
        yield return new WaitForSeconds(startSoundDelay);
        AudioSource.PlayClipAtPoint(startSound, transform.position);
    }
    
    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(colliderDelay);

        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(goDelay);

        gameObject.SetActive(false);
        
    }
}
