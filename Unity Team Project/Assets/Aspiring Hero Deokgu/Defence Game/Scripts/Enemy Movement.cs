using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 적 목표 설정 Script
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    NavMeshAgent myAgent;
    GameObject goal;
    float goalOffset = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        goal = GameObject.Find("Goal");

        myAgent.SetDestination(goal.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
