using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public float volumeAmbiant = 0.5f;

    public static SoundManager instance = null;                  
    public float lowPitchRange = .95f;              
    public float highPitchRange = 1.05f;            
    public AudioSource ambiantSource;

    public static SoundManager GetSoundManager()
    {
        return FindObjectOfType<SoundManager>();
    }

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void PlayEffect(AudioClip clip, float pitch)
    {
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.pitch = randomPitch ;
        audioSource.clip = clip ;
        audioSource.Play();
        Destroy(audioSource, clip.length + 1);
    }
    public void PlayAmbiant()
    {
        ambiantSource.Play();
    }
    public void PlaySound(AudioClip clip, GameObject go, float volume = 1)
    {
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.spatialize = true ;
        audioSource.volume = volume;
        // si spatialize marche, pas ce qui est fort probable supprimer la ligne au dessus et décommenter en dessous

        /*audioSource.spatialBlend = 1;
        audioSource.dopplerLevel = 0;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 50;*/

        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource, clip.length + 1);
    }
    public void PlaySoundRandomPitch(AudioClip clip, GameObject go, float minPitch, float maxPitch)
    {
        float randomPitch = Random.Range(minPitch, maxPitch);
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.pitch = randomPitch;
        audioSource.clip = clip;
        audioSource.Play();
        Destroy(audioSource, clip.length + 1);
    }
}
