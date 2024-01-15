using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FishingText : MonoBehaviour
{
    public static FishingText Instance;

    public Image FishingTutorial;
    public Text TutorialA;
    public Text TutorialB;
    public Button TutorialOffButton;

    public bool FishingA_Tutorial;
    public bool FishingB_Tutorial;


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        FishingTutorial.gameObject.SetActive(false);
        TutorialA.gameObject.SetActive(false);
        TutorialB.gameObject.SetActive(false);
        TutorialOffButton.gameObject.SetActive(false);
        FishingA_Tutorial = false;
        FishingB_Tutorial = false;
    }

    public void TutoA()
    {
        if (FishingA_Tutorial == false)
        {
            FishingA_Tutorial = true;
            FishingTutorial.gameObject.SetActive(true);
            TutorialA.gameObject.SetActive(true);
            TutorialOffButton.gameObject.SetActive(true);
        }
    }

    public void TutoB()
    {
        if (FishingB_Tutorial == false)
        {
            FishingB_Tutorial = true;
            FishingTutorial.gameObject.SetActive(true);
            TutorialB.gameObject.SetActive(true);
            TutorialOffButton.gameObject.SetActive(true);
        }
    }

    public void TutorialOff()
    {
        FishingTutorial.gameObject.SetActive(false);
        TutorialA.gameObject.SetActive(false);
        TutorialB.gameObject.SetActive(false);
        TutorialOffButton.gameObject.SetActive(false);
    }
}
