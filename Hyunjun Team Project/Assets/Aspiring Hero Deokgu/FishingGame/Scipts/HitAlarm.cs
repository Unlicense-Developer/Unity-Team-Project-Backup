using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HitAlarm : MonoBehaviour
{
    public TextMeshProUGUI textA;
    public static HitAlarm Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AnimateText()
    {
        textA.gameObject.SetActive(true);
        Vector3 initialPosition = textA.transform.position;

        // 텍스트 애니메이션: 좀 더 빠르게 이동하도록 변경
        textA.transform.DOMove(new Vector3(initialPosition.x + 50f, initialPosition.y + 0f, initialPosition.z), 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                textA.DOFade(0f, 1.0f).SetDelay(0.5f).OnComplete(() =>
                {
                    textA.gameObject.SetActive(false);
                });
            });
    }
}
