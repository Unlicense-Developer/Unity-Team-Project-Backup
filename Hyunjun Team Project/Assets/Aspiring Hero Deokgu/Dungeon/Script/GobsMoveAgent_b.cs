using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GobsMoveAgent_b : MonoBehaviour
{
    public Transform[] waypoints;   //waypoint���� ��ġ�� �迭�� ����
    public int nextIndex;   // waypoint�� �迭 Index��

    NavMeshAgent nav;
    Transform tr;
    readonly float patrolSpeed = 1.2f;
    readonly float chaseSpeed = 2f;
    float damping = 1f;     //ȸ���ҋ� �ӵ� ����(���)
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
            damping = 7f;   //���� ������ ���
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

        CacheWaypoints();   //ĳ�̵� ��������Ʈ�� ������
        MoveWaypoints();
    }

    private void CacheWaypoints()
    {
        GameObject waypointGroup = GameObject.Find("WaypointGroup_b");
        if (waypointGroup != null)
        {   // ��������Ʈ �׷��� ã���� �� ���� ��������Ʈ���� ĳ��
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
        if (nav.isPathStale) return;   // ���������� ��� ���
        if (waypoints == null || waypoints.Length == 0) return; //�迭�� ����ִ��� üũ
        nav.destination = waypoints[nextIndex].position;
        nav.isStopped = false;
    }

    public void ChasePlayer(Vector3 pos)
    {
        if (nav.isPathStale) return;    //�̵���θ� ��ã���� ������ �������� ����
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
        {   // NavMesh�� �̵��ϴ� ���� vector�� quaternion Ÿ���� angle�� ��ȯ������.
            Quaternion rot = Quaternion.LookRotation(nav.desiredVelocity);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
        }

        //if (!isPatrol) return;  //������尡 �ƴϸ� �ʱ�ȭ

        if (nav.velocity.sqrMagnitude >= 0.2f * 0.2f && nav.remainingDistance <= 0.5f)
        {
            nextIndex = Random.Range(0, waypoints.Length);
            MoveWaypoints();
        }
    }
}
