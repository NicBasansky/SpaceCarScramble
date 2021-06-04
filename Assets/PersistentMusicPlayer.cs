using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentMusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip introSynth;
    [SerializeField] AudioClip mainSynth;

    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = introSynth;
        audioSource.Play();
        DontDestroyOnLoad(this);
        
    }

    public void ChangeMusic()
    {
        audioSource.Stop();
        audioSource.clip = mainSynth;
        audioSource.Play();
    }

    
}
