using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Alchemy
{

    public class Circle : MonoBehaviour
    {
        public void Awake()
        {
            // ������Ʈ�� Transform ������Ʈ ��������
            Transform circleTransform = this.transform;

            // ������Ʈ�� ��ġ(Vector3) ��������
            Vector3 circlePosition = circleTransform.position;
        }
    }
}
