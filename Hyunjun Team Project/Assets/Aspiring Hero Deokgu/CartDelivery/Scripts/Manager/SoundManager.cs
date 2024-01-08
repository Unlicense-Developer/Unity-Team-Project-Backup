using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    //[SerializeField]
    public AudioSource gameSound;
    //[SerializeField]
    //AudioClip[] horseWalkSound;
    public AudioClip horseWalkSound;
    [SerializeField]
    Transform horse;
    void Awake()
    {
        if (SoundManager.instance == null)
            SoundManager.instance = this;
    }
    void Start()
    {
        gameSound.clip = horseWalkSound;
    }

    public void PlaySoundHorseWalk()
    {
        //AudioSource.PlayClipAtPoint(horseWalkSound, transform.TransformPoint(horse.position));

        gameSound.PlayOneShot(horseWalkSound);
        //gameSound.UnPause();
    }

    public void StopSoundHorseWalk()
    {
        //gameSound.Pause();
    }

    void Update()
    {

    }
}
