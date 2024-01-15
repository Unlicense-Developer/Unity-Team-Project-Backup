using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("캠트리거 충돌");
            EventCameraController.Instacne.ActiveEventRoomCamera();
        }
    }
}
