using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace CartDelivery
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        CinemachineVirtualCameraBase camera1;
        [SerializeField]
        CinemachineVirtualCameraBase camera2;
        bool isCamera2Active = false;

        void Start()
        {

        }
        void CameraChange()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isCamera2Active = !isCamera2Active;
                if (isCamera2Active)
                {
                    camera1.Priority = 10;
                    camera2.Priority = 15;
                }
                else
                {
                    camera1.Priority = 15;
                    camera2.Priority = 10;
                }
            }

        }
        void Update()
        {
            CameraChange();
        }
    }
}

