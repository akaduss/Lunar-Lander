using System;
using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioClip thrustSound;
    [SerializeField] private AudioSource audioSource;
    private bool isThrusting;
    private void Start()
    {
        audioSource.clip = thrustSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        SoundManager.Instance.OnSoundVolumeChanged += (object sender, EventArgs e) =>
        {
            audioSource.volume = SoundManager.Instance.GetSoundVolumeNormalized();
        };
        Lander.Instance.OnBeforeForce += Instance_OnBeforeForce;
        Lander.Instance.OnUpKeyPressed += Instance_OnUpKeyPressed;
        Lander.Instance.OnLeftKeyPressed += Instance_OnLeftKeyPressed;
        Lander.Instance.OnRightKeyPressed += Instance_OnRightKeyPressed;

    }

    private void Instance_OnUpKeyPressed(object sender, EventArgs e)
    {
        isThrusting = true;
    }

    private void Instance_OnLeftKeyPressed(object sender, EventArgs e)
    {
        isThrusting = true;
    }

    private void Instance_OnRightKeyPressed(object sender, EventArgs e)
    {
        isThrusting = true;
    }

    private void Instance_OnBeforeForce(object sender, System.EventArgs e)
    {
        isThrusting = false;
    }

    private void Update()
    {
        if (isThrusting)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
