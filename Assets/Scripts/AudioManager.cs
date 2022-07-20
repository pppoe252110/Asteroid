using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    private static AudioClip clip;
    private static AudioManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static void PlayAudioClip(AudioClip clip)
    {
        instance.ProcessAudio(clip);
    }

    private void ProcessAudio(AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(source, clip.length);
    }
}
