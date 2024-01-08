using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Alchemy
{

    public class Circle : MonoBehaviour
    {
        public void Awake()
        {
            // 오브젝트의 Transform 컴포넌트 가져오기
            Transform circleTransform = this.transform;

            // 오브젝트의 위치(Vector3) 가져오기
            Vector3 circlePosition = circleTransform.position;
        }
    }
}
