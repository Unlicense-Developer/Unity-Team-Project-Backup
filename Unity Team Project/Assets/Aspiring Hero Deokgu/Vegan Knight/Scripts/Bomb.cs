using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    AudioSource bombSound;

    public AudioClip fuzeSound;
    public AudioClip explodeSound;

    private void Start()
    {
        bombSound = GetComponent<AudioSource>();
        bombSound.clip = fuzeSound;
        bombSound.Play();
    }

    private void Update()
    {
        if (gameObject.transform.position.y <= -15.0f)
            bombSound.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            bombSound.clip = explodeSound;
            bombSound.Play();
            VeganNinjaManager.Instance.Explode();
        }
    }

}
