using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GetPotions();
            }
        }
    }

    private void GetPotions()
    {
        GetPotion.Instance.GetP();
        WorldSoundManager.Instance.PlaySFX("GetPoionSound");
    }
}
