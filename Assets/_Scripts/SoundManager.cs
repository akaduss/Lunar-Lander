using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public event EventHandler OnSoundVolumeChanged;

    private const int MAX_VOLUME = 10;

    [SerializeField] private AudioClip fuelPickupSound;
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip thrustSound;
    [SerializeField] private AudioClip crashSound;
    [SerializeField] private AudioClip successSound;

    private static int soundVolume = 6; // 0 - 10

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        if(e.landingType == Lander.LandingType.Success)
        {
            AudioSource.PlayClipAtPoint(successSound, Camera.main.transform.position, GetSoundVolumeNormalized());
        }
        else
        {
            AudioSource.PlayClipAtPoint(crashSound, Camera.main.transform.position, GetSoundVolumeNormalized());
        }
    }

    private void Lander_OnFuelPickup(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickupSound, Camera.main.transform.position, GetSoundVolumeNormalized());
    }

    private void Lander_OnCoinPickup(object sender, EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position, GetSoundVolumeNormalized());
    }

    public void ChangeSoundVolume()
    {
        soundVolume = (soundVolume + 1) % MAX_VOLUME;
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int GetSoundVolume()
    {
        return soundVolume;
    }

    public float GetSoundVolumeNormalized()
    {
        return (float)soundVolume / MAX_VOLUME;
    }
}
