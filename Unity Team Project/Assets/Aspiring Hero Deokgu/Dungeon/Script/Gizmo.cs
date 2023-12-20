using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public Color colorA = Color.yellow;
    public float radiusA = 0.1f;

    private void OnDrawGizmos()     // Gizmo가 시각적으로 보이게 해줌
    {
        Gizmos.color = colorA;
        Gizmos.DrawSphere(transform.position, radiusA);     //(생성위치, 반지름)
    }
}
