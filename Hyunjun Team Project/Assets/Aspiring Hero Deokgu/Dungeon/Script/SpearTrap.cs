using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpearTrap : MonoBehaviour
{
    public float moveDistance = 2f;
    public float moveDuration = 0.2f;
    public float delayBetweenMovements = 2f;

    void Start()
    {
        StartCoroutine(MoveRepeatedly());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }

    IEnumerator MoveRepeatedly()
    {
        while (true)
        {
            while (true)
            {
                yield return new WaitForSeconds(delayBetweenMovements);

                transform.DOMoveY(transform.position.y + moveDistance, moveDuration)
                    .SetEase(Ease.Linear);

                yield return new WaitForSeconds(2f); // 1√  ¥Î±‚

                transform.DOMoveY(transform.position.y - moveDistance, moveDuration)
                    .SetEase(Ease.Linear);
            }
        }
    }
}
