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
            // 3�� �Ŀ� �ؽ�Ʈ�� ������ ������� �ִϸ��̼�
            DOVirtual.DelayedCall(3f, () =>
            {
                Event1Text.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    // ����� �Ŀ� �� �۾����� ���⿡ �߰��� �� �ֽ��ϴ�.
                });
            });
        });
    }

    public void EventCameraTextB()
    {
        Event2Text.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // 3�� �Ŀ� �ؽ�Ʈ�� ������ ������� �ִϸ��̼�
            DOVirtual.DelayedCall(3f, () =>
            {
                Event2Text.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    // ����� �Ŀ� �� �۾����� ���⿡ �߰��� �� �ֽ��ϴ�.
                });
            });
        });
    }
    public void FallInDarkText()
    {
        FallText.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            // 3�� �Ŀ� �ؽ�Ʈ�� ������ ������� �ִϸ��̼�
            DOVirtual.DelayedCall(3f, () =>
            {
                FallText.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    // ����� �Ŀ� �� �۾����� ���⿡ �߰��� �� �ֽ��ϴ�.
                });
            });
        });
    }
}