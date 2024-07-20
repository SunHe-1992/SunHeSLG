using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Inst;
    AudioSource musicSource;
    private void Awake()
    {
        if (MusicManager.Inst == null)
        {
            MusicManager.Inst = this;
            DontDestroyOnLoad(gameObject);
            musicSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        musicSource.Play();
        musicSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePlayManager.Instance != null)
        {
            musicSource.volume = GamePlayManager.Instance.gameSetting.musicVolume;
        }

    }

}
