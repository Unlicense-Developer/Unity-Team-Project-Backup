using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dungeon;


public class RunePad : MonoBehaviour
{
    public PentagramGate gateController;

    public bool isActivated = false; // ����ġ Ȱ��ȭ ����

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isActivated = true;
            transform.DOMoveY(this.transform.position.y - 0.5f, 3f);

            WorldSoundManager.Instance.PlaySFX("Pad");

            gateController.DeviceTriggered();
        }
    }
}
