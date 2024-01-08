using UnityEngine;


namespace Alchemy
{
public class RotateSprite : MonoBehaviour
{
    public float rotationSpeed = -20.0f; // 회전 속도 (60도/초로 설정되어 있음)

    void Update()
    {
        // 스프라이트의 현재 회전 각도를 가져옴
        float currentRotation = transform.rotation.eulerAngles.z;

        // 회전 각도를 업데이트하여 시계방향으로 회전하도록 함
        currentRotation += rotationSpeed * Time.deltaTime;

        // 360도를 넘어가면 다시 0으로 초기화
        if (currentRotation >= 360.0f)
        {
            currentRotation -= 360.0f;
        }

        // 스프라이트에 새로운 회전 각도를 적용
        transform.rotation = Quaternion.Euler(0, 0, currentRotation);
    }
    }
}
