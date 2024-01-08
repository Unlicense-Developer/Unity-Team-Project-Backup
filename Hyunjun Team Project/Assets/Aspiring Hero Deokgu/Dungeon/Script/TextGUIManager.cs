using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class TextGUIManager : MonoBehaviour
{
    public static TextGUIManager Instance;

    public Text Event1Text;
    public Text Event2Text;
    public Text FallText;

    public float Duration_A = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        Event1Text.color = new Color(Event1Text.color.r, Event1Text.color.g, Event1Text.color.b, 0f);

    }

    public void EventCameraTextA()
    {
        Event1Text.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // 3초 후에 텍스트가 서서히 사라지는 애니메이션
            DOVirtual.DelayedCall(3f, () =>
            {
                Event1Text.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    // 사라진 후에 할 작업들을 여기에 추가할 수 있습니다.
                });
            });
        });
    }

    public void EventCameraTextB()
    {
        Event2Text.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // 3초 후에 텍스트가 서서히 사라지는 애니메이션
            DOVirtual.DelayedCall(3f, () =>
            {
                Event2Text.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    // 사라진 후에 할 작업들을 여기에 추가할 수 있습니다.
                });
            });
        });
    }
    public void FallInDarkText()
    {
        FallText.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // 3초 후에 텍스트가 서서히 사라지는 애니메이션
            DOVirtual.DelayedCall(3f, () =>
            {
                FallText.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    // 사라진 후에 할 작업들을 여기에 추가할 수 있습니다.
                });
            });
        });
    }
}