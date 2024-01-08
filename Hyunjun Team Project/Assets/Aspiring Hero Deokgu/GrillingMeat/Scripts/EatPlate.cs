using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrillingMeatGame
{
    public class EatPlate : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("MEAT")) //고기가 닿으면 
            {
                other.gameObject.SetActive(false); //사라지게 한다
            }
        }
    }
}

