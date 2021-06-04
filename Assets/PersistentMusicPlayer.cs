using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentMusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip introSynth;
    [SerializeField] AudioClip mainSynth;

    AudioSource audioSource;
    bool introIsPlaying = false;
    bool readyToSwitch = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = introSynth;
        audioSource.Play();
        DontDestroyOnLoad(this);
        
    }

    public void ChangeMusic()
    {
        readyToSwitch = true;
        
    }


    private void Update() 
    {
        //print("time: " + audioSource.time + " length: " + audioSource.clip.length);
        if (readyToSwitch)
        {
            
            audioSource.clip = mainSynth;
            audioSource.Play();
            StartCoroutine(AlternateClips());
            readyToSwitch = false;
            
        }
    }

    IEnumerator AlternateClips()
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        
        audioSource.clip = introSynth;
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        readyToSwitch = true;

    }

    
}
