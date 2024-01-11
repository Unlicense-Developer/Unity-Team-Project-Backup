using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Enemy ±âº» Script
/// </summary>
public class EnemyBase : MonoBehaviour
{
    EnemySound sound;
    NavMeshAgent myAgent;
    Animator animator;

    int hp;
    int killScore;

    bool isDead;
    bool isDamaged;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.tag == "Goblin")
        {
            hp = 1;
            killScore = 10;
        }
        else if (gameObject.tag == "Orc")
        {
            hp = 2;
            killScore = 30;
        }
        else if (gameObject.tag == "Troll")
        {
            hp = 3;
            killScore = 50;
        }

        sound = GetComponent<EnemySound>();
        myAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDead();
    }

    public void Hit()
    {
        hp -= 1;

        if (hp <= 0)
        {
            isDead = true;
            animator.SetBool("isDead", isDead);
        }
        else
        {
            PlayHit();
        }

    }

    void PlayHit()
    {
        sound.PlayDamagedSound();
        isDamaged = true;
        animator.SetBool("isDamaged", isDamaged);
        animator.SetInteger("Anim State Num", Random.Range(1, 5));
        myAgent.isStopped = true;
    }

    void CheckDead()
    {
        if(isDead && animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Run"))
        {
            animator.SetInteger("Anim State Num", Random.Range(1, 5));
            myAgent.isStopped = true;
        }
    }

    public void PlayDead()
    {
        sound.PlayDeathSound();
        DefenceGameManager.Instance.AddScore(killScore);
        StartCoroutine(DeleteAfterSeconds());
    }

    public void ResetAnimation()
    {
        isDamaged = false;
        animator.SetBool("isDamaged", isDamaged);
        animator.SetInteger("Anim State Num", 0);
        myAgent.isStopped = false;
    }

    IEnumerator DeleteAfterSeconds()
    {
        yield return new WaitForSeconds(6.0f);

        Destroy(gameObject);
    }
}
