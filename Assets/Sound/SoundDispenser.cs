using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDispenser : MonoBehaviour
{
    public List<AudioSource> sources;


    public void Play(AudioClip clip)
    {
        for (int i = 0; i < sources.Count; i++)
        {
            sources[i].enabled = true;
            gameObject.SetActive(true);
            if (!sources[i].isPlaying && sources[i].enabled && gameObject.activeSelf)
            {
                sources[i].clip = clip;
                sources[i].Play();
                break;
            }
        }

    }
}
