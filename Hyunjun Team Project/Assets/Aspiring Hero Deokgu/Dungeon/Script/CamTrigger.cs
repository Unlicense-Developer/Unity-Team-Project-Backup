using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("ķƮ���� �浹");
            EventCameraController.Instacne.ActiveEventRoomCamera();
        }
    }
}
