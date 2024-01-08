using UnityEngine;


namespace Alchemy
{
public class RotateSprite : MonoBehaviour
{
    public float rotationSpeed = -20.0f; // ȸ�� �ӵ� (60��/�ʷ� �����Ǿ� ����)

    void Update()
    {
        // ��������Ʈ�� ���� ȸ�� ������ ������
        float currentRotation = transform.rotation.eulerAngles.z;

        // ȸ�� ������ ������Ʈ�Ͽ� �ð�������� ȸ���ϵ��� ��
        currentRotation += rotationSpeed * Time.deltaTime;

        // 360���� �Ѿ�� �ٽ� 0���� �ʱ�ȭ
        if (currentRotation >= 360.0f)
        {
            currentRotation -= 360.0f;
        }

        // ��������Ʈ�� ���ο� ȸ�� ������ ����
        transform.rotation = Quaternion.Euler(0, 0, currentRotation);
    }
    }
}
