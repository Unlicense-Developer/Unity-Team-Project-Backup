using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenShopUI : MonoBehaviour
{
    public GameObject shopButton;
    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.tag == "Player")
        {
            shopButton.SetActive(true);
        }
    }
}
