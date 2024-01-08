using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GobsAI : MonoBehaviour
{
    public enum STATE
    {
        PATROL,
        CHASE,
        DIE
    }

    public STATE state = STATE.PATROL;

    Animator anim;
    Transform PlayerT;
    Transform GobsT;
    GobsMoveAgent moveAgent;
    EnemyFOV enemyFOV;

    public bool isDie = false;
    public float chaseDis = 5f;
    public float rechaseDis = 4f;

    readonly int hashIsMove = Animator.StringToHash("IsMove");
    readonly int hashDie = Animator.StringToHash("IsDaed");
    readonly int hashSpeed = Animator.StringToHash("Speed");

    private void Awake()
    {
        PlayerT = GameObject.FindWithTag("Player").GetComponent<Transform>();
        GobsT = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        moveAgent = GetComponent<GobsMoveAgent>();
        enemyFOV = GetComponent<EnemyFOV>();
    }

    void Start()
    {
        StartCoroutine(UpdateState());
        StartCoroutine(CheckState());
    }

    IEnumerator CheckState()
    {
        while (!isDie)
        {
            if (state == STATE.DIE) yield break;    //coroutine 종료

            float distance = Vector3.Distance(PlayerT.position, GobsT.position);
            //float distance = (PlayerT.position - GobsT.position).sqrMagnitude;   //위에 사용한 방법보다 최적화된 방법

            if (distance <= chaseDis)
            {
                if (enemyFOV.IsChasePlayer())
                    state = STATE.CHASE;
            }
            else
            {
                state = STATE.PATROL;
            }
            yield return 0.3f;
        }
    }

    void patrol()
    {
        moveAgent.IsPatrol = true;
        anim.SetBool(hashIsMove, true);
    }

    void chase()
    {
        moveAgent.ChaseTarget = PlayerT.position;
        anim.SetBool(hashIsMove, true);
    }
    void die()
    {
        moveAgent.Stop();
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.SetActive(false);
        anim.SetTrigger(hashDie);
    }

    IEnumerator UpdateState()
    {
        while (!isDie)
        {
            switch (state)
            {
                case STATE.PATROL:
                    patrol();
                    break;
                case STATE.CHASE:
                    chase();
                    break;
                case STATE.DIE:
                    die();
                    break;
            }
            yield return 0.3f;
        }
    }
}
