using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizmo : MonoBehaviour
{
    public Color colorA = Color.yellow;
    public float radiusA = 0.1f;

    private void OnDrawGizmos()     // Gizmo�� �ð������� ���̰� ����
    {
        Gizmos.color = colorA;
        Gizmos.DrawSphere(transform.position, radiusA);     //(������ġ, ������)
    }
}
