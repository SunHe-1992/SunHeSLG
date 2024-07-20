using UnityEngine.Audio;
using UnityEngine;
using System;

[Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    public bool loop=false;

    [Range(0, 1)]
    public float volume=1;

    [Range(0.1f, 3)]
    public float pitch;

    [HideInInspector]
    public AudioSource source;
}
