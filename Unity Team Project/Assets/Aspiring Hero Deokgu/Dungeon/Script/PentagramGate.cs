using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PentagramGate : MonoBehaviour
{
    public float openHeight = 4.5f; // 게이트가 열린 높이
    public float openDuration = 10f; // 게이트 열리는 시간

    private int ActivatedDevices = 0; // 충돌한 장치 수

    private Vector3 initialPosition; // 초기 위치
    private Vector3 targetPosition; // 목표 위치

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * openHeight; // 열린 위치 설정
    }
    public void CheckAndOpenGate()
    {
        if (ActivatedDevices >= 4)
        {
            transform.DOMove(targetPosition, openDuration);     // DoTween을 사용하여 게이트 열기
        }
    }

    public void DeviceTriggered()
    {
        ActivatedDevices++;
        CheckAndOpenGate();
    }
}
