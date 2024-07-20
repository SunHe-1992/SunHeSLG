using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public Dictionary<string, Sound> soundDic;
    public static AudioManager Inst;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //DontDestroyOnLoad( gameObject );

        soundDic = new Dictionary<string, Sound>();
        foreach (Sound sound in sounds)
        {
            if (sound.clip != null)
            {
                sound.source = gameObject.AddComponent<AudioSource>(); //creates an audiosource

                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                print($"{sound.name} was successfully loaded");
                soundDic[sound.name] = sound;
            }
        }
    }
    public void Play(string name, bool interrupt = true)
    {
        if (interrupt == false && IsPlaying(name)) //don't interrupt, the music is already playing 
        {
            return;
        }
        Sound sound = null;
        if (soundDic.TryGetValue(name, out sound) == false)
        {
            print($"Sound: {name} not found");
            return;
        }
        sound.source.Stop();
        sound.source.Play();
        print("Play sound " + name);
    }
    public void Stop(string name)
    {
        Sound sound = null;
        if (soundDic.TryGetValue(name, out sound) == false)
        {
            print($"Sound: {name} not found");
            return;
        }
        sound.source.Stop();
    }
    public bool IsPlaying(string name)
    {
        Sound sound = null;
        if (soundDic.TryGetValue(name, out sound) == false)
        {
            print($"Sound: {name} not found");
            return false;
        }
        return sound.source.isPlaying;
    }
}
