using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySound : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip death;
    public AudioClip damaged;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDamagedSound()
    {
        audioSource.PlayOneShot(damaged);
    }

    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(death);
    }
}
