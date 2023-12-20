using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInOut : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public Image image;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {

    }

    public void FadeIn()
    {
        StartCoroutine(Fade(true));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool isFadeIn)
    {
        gameObject.SetActive(true);

        if (isFadeIn)
        {
            canvasGroup.alpha = 1;
            Tween tween = canvasGroup.DOFade(0f, 1f);
            yield return tween.WaitForCompletion();
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(true);
            Tween tween = canvasGroup.DOFade(1f, 1f);
            yield return tween.WaitForCompletion();
        }
    }
}
