using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrillingMeatGame
{
    public class Background : MonoBehaviour
    {
        void Start()
        {
        }
        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.gameObject.CompareTag("MEAT")) //고기가 닿으면 
            {
                GameManager.instance.SoundMeatStop();//고기 소리 재생멈춤 
                GameManager.instance.SoundMeatStopLoop();//고기 소리 루프 끔
            }
        }
    }
}