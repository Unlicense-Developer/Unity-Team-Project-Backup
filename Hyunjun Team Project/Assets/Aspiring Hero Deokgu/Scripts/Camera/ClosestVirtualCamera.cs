using UnityEngine;
using Cinemachine;

public class ClosestVirtualCamera : MonoBehaviour
{
    public Transform playerTransform; // 플레이어 위치
    private CinemachineClearShot clearShotCamera;

    void Start()
    {
        clearShotCamera = GetComponent<CinemachineClearShot>();
        if (clearShotCamera == null)
        {
            Debug.LogError("CinemachineClearShot 컴포넌트가 필요합니다.");
        }

        if (playerTransform == null)
        {
            Debug.LogError("플레이어 Transform이 할당되어야 합니다.");
        }
    }

    void Update()
    {
        if (clearShotCamera != null && playerTransform != null)
        {
            SelectClosestVirtualCamera();
        }
    }

    void SelectClosestVirtualCamera()
    {
        float closestDistance = float.MaxValue;
        CinemachineVirtualCamera closestCamera = null;

        foreach (CinemachineVirtualCamera vcam in clearShotCamera.ChildCameras)
        {
            float distance = Vector3.Distance(playerTransform.position, vcam.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCamera = vcam;
            }
        }

        if (closestCamera != null)
        {
            foreach (CinemachineVirtualCamera vcam in clearShotCamera.ChildCameras)
            {
                vcam.Priority = (vcam == closestCamera) ? 1 : 0;
            }
        }
    }
}
