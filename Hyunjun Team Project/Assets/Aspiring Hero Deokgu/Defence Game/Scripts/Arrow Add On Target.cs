using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ȭ���� Ÿ�ٿ� �ٰ� �ϴ� ���, ���� �ð� ������ ȭ�� �������� ���
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
        //�¾Ҵٸ� ���� �ð� �� ȭ�� ����
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

        //�¾����� ���ߵ��� ó��
        rigid.isKinematic = true;
        transform.SetParent(collision.transform);
    }

    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(lifeTime);

        Destroy(gameObject);
    }
}
