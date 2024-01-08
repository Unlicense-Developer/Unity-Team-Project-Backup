using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GobsMoveAgent_b : MonoBehaviour
{
    public Transform[] waypoints;   //waypoint들의 위치를 배열로 선언
    public int nextIndex;   // waypoint의 배열 Index값

    NavMeshAgent nav;
    Transform tr;
    readonly float patrolSpeed = 1.2f;
    readonly float chaseSpeed = 2f;
    float damping = 1f;     //회전할떄 속도 변수(계수)
    bool isPatrol;

    public bool IsPatrol
    {
        get { return isPatrol; }
        set
        {
            if (nav == null) return;
            isPatrol = value;

            if (isPatrol)
            {
                nav.speed = patrolSpeed;
                damping = 1f;
                MoveWaypoints();
            }
        }
    }

    Vector3 chaseTarget;
    public Vector3 ChaseTarget
    {
        get { return chaseTarget; }
        set
        {
            chaseTarget = value;
            damping = 7f;   //추적 상태의 계수
            nav.speed = chaseSpeed;
            ChasePlayer(chaseTarget);
        }
    }

    public float speed
    {
        get { return nav.velocity.magnitude; }
    }

    void Start()
    {
        tr = GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        nav.autoBraking = false;
        nav.updateRotation = false;
        nav.speed = patrolSpeed;

        CacheWaypoints();   //캐싱된 웨이포인트를 가져옴
        MoveWaypoints();
    }

    private void CacheWaypoints()
    {
        GameObject waypointGroup = GameObject.Find("WaypointGroup_b");
        if (waypointGroup != null)
        {   // 웨이포인트 그룹을 찾으면 그 하위 웨이포인트들을 캐싱
            Transform[] allWaypoints = waypointGroup.GetComponentsInChildren<Transform>();
            int totalWaypoints = allWaypoints.Length;

            if (totalWaypoints > 1)
            {
                waypoints = new Transform[totalWaypoints - 1];
                for (int i = 1; i < totalWaypoints; i++)
                {
                    waypoints[i - 1] = allWaypoints[i];
                }
            }
        }
    }

    public void MoveWaypoints()
    {   
        if (nav.isPathStale) return;   // 목적지까지 경로 계산
        if (waypoints == null || waypoints.Length == 0) return; //배열이 비어있는지 체크
        nav.destination = waypoints[nextIndex].position;
        nav.isStopped = false;
    }

    public void ChasePlayer(Vector3 pos)
    {
        if (nav.isPathStale) return;    //이동경로를 못찾으면 뒤쪽은 동작하지 않음
        nav.destination = pos;
        nav.isStopped = false;
    }

    public void Stop()
    {
        IsPatrol = false;
        nav.isStopped = true;
        nav.velocity = Vector3.zero;
    }


    void Update()
    {
        if (nav.isStopped == false)
        {   // NavMesh가 이동하는 방향 vector를 quaternion 타입이 angle로 변환시켜줌.
            Quaternion rot = Quaternion.LookRotation(nav.desiredVelocity);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
        }

        //if (!isPatrol) return;  //순찰모드가 아니면 초기화

        if (nav.velocity.sqrMagnitude >= 0.2f * 0.2f && nav.remainingDistance <= 0.5f)
        {
            nextIndex = Random.Range(0, waypoints.Length);
            MoveWaypoints();
        }
    }
}
