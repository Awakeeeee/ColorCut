using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgSource;
    public AudioSource gameSource;

    public void PlayInGameClip(AudioClip clip, float volume = 1f)
    {
        gameSource.PlayOneShot(clip, volume);
    }
}
