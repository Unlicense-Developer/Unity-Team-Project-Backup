using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRange = 5f; // 시야 거리
    [Range(0, 360)]
    public float viewAngle = 120f; // 시야각

    Transform playerTr;
    Transform enemyTr;

    void Awake()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 Transform을 가져옵니다.
        enemyTr = transform; // 적의 Transform을 가져옵니다.
    }

    public bool IsChasePlayer()
    {
        bool isChase = false;

        // 몬스터에서 플레이어까지의 방향을 계산
        Vector3 dir = (playerTr.position - enemyTr.position).normalized;

        // 플레이어가 시야 범위 내에 있고 시야각 내에도 있다면 추적 실행
        if (Vector3.Distance(playerTr.position, enemyTr.position) <= viewRange && Vector3.Angle(enemyTr.forward, dir) <= viewAngle * 0.5f)
        {
            // 플레이어와 사이에 장애물이 없으면 실행
            if (!IsViewPlayer())
            {
                isChase = true; // 플레이어를 추적합니다.
                Debug.Log("Chasing Player!");
            }
        }

        return isChase;
    }

    // 몬스터와 플레이어 사이에 장애물이 있는지 감지하는 함수
    public bool IsViewPlayer()
    {
        RaycastHit hit;
        Vector3 dir = (playerTr.position - enemyTr.position).normalized;

        // 몬스터에서 플레이어로 레이캐스트를 쏩니다.
        if (Physics.Raycast(enemyTr.position, dir, out hit, viewRange))
        {
            // 레이캐스트가 감지한 오브젝트가 플레이어가 아니라면
            if (!hit.collider.CompareTag("Player"))
            {
                return true; // 장애물이 있음을 반환합니다.
            }
        }
        return false; // 장애물이 없음을 반환합니다.
    }

    void OnDrawGizmos() //Gizmo를 이용하여 시야각과 범위를 Sceneview에서 확인
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