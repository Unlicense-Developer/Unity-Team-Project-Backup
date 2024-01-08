using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 5f; // �þ� �Ÿ�
    [Range(0, 360)]
    public float viewAngle = 120f; // �þ߰�

    Transform playerTr;
    Transform enemyTr;

    void Awake()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� Transform�� �����ɴϴ�.
        enemyTr = transform; // ���� Transform�� �����ɴϴ�.
    }

    public bool IsChasePlayer()
    {
        bool isChase = false;

        // ���Ϳ��� �÷��̾������ ������ ���
        Vector3 dir = (playerTr.position - enemyTr.position).normalized;

        // �÷��̾ �þ� ���� ���� �ְ� �þ߰� ������ �ִٸ� ���� ����
        if (Vector3.Distance(playerTr.position, enemyTr.position) <= viewRange && Vector3.Angle(enemyTr.forward, dir) <= viewAngle * 0.5f)
        {
            // �÷��̾�� ���̿� ��ֹ��� ������ ����
            if (!IsViewPlayer())
            {
                isChase = true; // �÷��̾ �����մϴ�.
                Debug.Log("Chasing Player!");
            }
        }

        return isChase;
    }

    // ���Ϳ� �÷��̾� ���̿� ��ֹ��� �ִ��� �����ϴ� �Լ�
    public bool IsViewPlayer()
    {
        RaycastHit hit;
        Vector3 dir = (playerTr.position - enemyTr.position).normalized;

        // ���Ϳ��� �÷��̾�� ����ĳ��Ʈ�� ���ϴ�.
        if (Physics.Raycast(enemyTr.position, dir, out hit, viewRange))
        {
            // ����ĳ��Ʈ�� ������ ������Ʈ�� �÷��̾ �ƴ϶��
            if (!hit.collider.CompareTag("Player"))
            {
                return true; // ��ֹ��� ������ ��ȯ�մϴ�.
            }
        }
        return false; // ��ֹ��� ������ ��ȯ�մϴ�.
    }

    void OnDrawGizmos() //Gizmo�� �̿��Ͽ� �þ߰��� ������ Sceneview���� Ȯ��
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRange);

        Vector3 fovLine1 = Quaternion.AngleAxis(viewAngle * 0.5f, transform.up) * transform.forward * viewRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-viewAngle * 0.5f, transform.up) * transform.forward * viewRange;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + fovLine1);
        Gizmos.DrawLine(transform.position, transform.position + fovLine2);
    }
}