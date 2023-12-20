using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 화살이 타겟에 붙게 하는 기능, 일정 시간 지나고 화살 없어지는 기능
/// </summary>
public class ArrowAddOnTarget : MonoBehaviour
{
    Rigidbody rigid;

    float lifeTime = 4.0f;
    bool targetHit;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //맞았다면 일정 시간 후 화살 삭제
        if (targetHit)
            StartCoroutine(DestroyArrow());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit)
            return;
        else
            targetHit = true;

        if( collision.gameObject.GetComponent<EnemyBase>() != null)
        {
            EnemyBase enemy = collision.gameObject.GetComponent<EnemyBase>();
            enemy.Hit();
        }

        //맞았을때 멈추도록 처리
        rigid.isKinematic = true;
        transform.SetParent(collision.transform);
    }

    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }
}
