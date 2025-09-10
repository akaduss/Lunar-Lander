using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private const int MAX_VOLUME = 10;

    private static float musicTime;
    private static int musicVolume = 6; // 0 - 10
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        audioSource.time = musicTime;
        audioSource.volume = GetMusicVolumeNormalized();
    }

    private void Update()
    {
        musicTime = audioSource.time;
    }

    public void ChangeMusicVolume()
    {
        musicVolume = (musicVolume + 1) % MAX_VOLUME;
        audioSource.volume = GetMusicVolumeNormalized();
    }

    public int GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetMusicVolumeNormalized()
    {
        return (float)musicVolume / MAX_VOLUME;
    }
}
