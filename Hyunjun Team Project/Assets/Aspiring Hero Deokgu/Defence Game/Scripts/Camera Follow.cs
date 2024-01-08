using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 카메라 플레이어 위치 이동
/// </summary>
public class CameraFollow : MonoBehaviour
{
    public Transform camPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = camPos.position;
    }
}
