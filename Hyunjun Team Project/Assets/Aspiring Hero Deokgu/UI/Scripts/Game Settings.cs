using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource audioSource;
        

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volumeSlider.value;
    }

    public void SetScreen1920()
    {
        Screen.SetResolution(1920, 1080, false);
    }

    public void SetScreen1280()
    {
        Screen.SetResolution(1280, 720, false);
    }
}
