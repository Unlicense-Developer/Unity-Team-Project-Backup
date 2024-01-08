using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PentagramTrigger : MonoBehaviour
{
    public GameObject ring;
    private bool isTriggerActivated = false;

    void Start()
    {
        ring.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggerActivated)
        {
            BossActive.instance.DeviceActivated();
            StartCoroutine(ringEffect());
            isTriggerActivated = true;
        }
        else
        {
            Debug.Log("�� �̻� �۵����� �ʴ´�");
        }
    }

    IEnumerator ringEffect()
    {
        ring.SetActive(true);

        yield return new WaitForSeconds(30f);

        ring.SetActive(false);
    }

    void Update()
    {
        
    }
}
