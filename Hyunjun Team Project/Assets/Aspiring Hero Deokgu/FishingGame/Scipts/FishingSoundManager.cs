using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingSoundManager : MonoBehaviour
{
    public AudioClip castSound;
    public AudioClip catchSound;
    public AudioClip successSound;
    public AudioClip failSound;
    public AudioClip reelInSound;

    private AudioSource audioSource;

    private bool isReelInPlaying;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCastSound()
    {
        PlaySound(castSound);
    }

    public void PlayCatchSound()
    {
        PlaySound(catchSound);
    }

    public void PlaySuccessSound()
    {
        PlaySound(successSound);
    }

    public void PlayFailSound()
    {
        PlaySound(failSound);
    }

    public void PlayReelInSound()
    {
        if (!isReelInPlaying)
        {
            isReelInPlaying = true;
            audioSource.loop = true;
            audioSource.clip = reelInSound;
            audioSource.Play();
        }
    }

    public void StopReelInSound()
    {
        isReelInPlaying = false;
        audioSource.loop = false;
        audioSource.Stop();
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}

