using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dungeon;

public class PentagramGate : MonoBehaviour
{
    public float openHeight = 4.5f; // ����Ʈ�� ���� ����
    public float openDuration = 10f; // ����Ʈ ������ �ð�

    public int ActivatedDevices = 0; // �۵��� ��ġ ��

    private Vector3 initialPosition; // �ʱ� ��ġ
    private Vector3 targetPosition; // ��ǥ ��ġ

    public GameObject dust;


    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + Vector3.up * openHeight; // ���� ��ġ ����
        dust.SetActive(false);
    }

    public void DeviceTriggered()
    {
        ActivatedDevices++;
        Debug.Log(ActivatedDevices);
        CheckAndOpenGate();
    }

    public void CheckAndOpenGate()
    {
        if (ActivatedDevices >= 4)
        {
            GateMoveUp();
            EventCameraController.Instacne.EventOn();
            StartCoroutine(SFX());
            StartCoroutine(DustFall());
        }
    }

    IEnumerator DustFall()
    {
        dust.SetActive(true);

        yield return new WaitForSeconds(openDuration);

        dust.SetActive(false);
    }

    IEnumerator SFX()
    {
        yield return new WaitForSeconds(0.5f);
        DungeonSoundManager.Instance.PlaySFX("GateOpenSound");
    }



    private void GateMoveUp()
    {
        transform.DOMove(targetPosition, openDuration);     // DoTween�� ����Ͽ� ����Ʈ ����
    }

}
