using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PentagramGate : MonoBehaviour
{
    public float openHeight = 4.5f; // ����Ʈ�� ���� ����
    public float openDuration = 10f; // ����Ʈ ������ �ð�

    private int ActivatedDevices = 0; // �浹�� ��ġ ��

    private Vector3 initialPosition; // �ʱ� ��ġ
    private Vector3 targetPosition; // ��ǥ ��ġ

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * openHeight; // ���� ��ġ ����
    }
    public void CheckAndOpenGate()
    {
        if (ActivatedDevices >= 4)
        {
            transform.DOMove(targetPosition, openDuration);     // DoTween�� ����Ͽ� ����Ʈ ����
        }
    }

    public void DeviceTriggered()
    {
        ActivatedDevices++;
        CheckAndOpenGate();
    }
}
