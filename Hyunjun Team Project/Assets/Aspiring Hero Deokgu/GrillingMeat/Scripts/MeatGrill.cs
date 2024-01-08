using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrillingMeatGame
{
    public class MeatGrill : MonoBehaviour
    {
        bool haveMeat = false;
        private void Start()
        {
            haveMeat = false;
        }
        void MeatSoundOnOffSign()
        {
            if (haveMeat == true)
                GameManager.instance.SoundMeatPlay.Invoke(); //고기 소리 재생 
            if (haveMeat == false)
                GameManager.instance.SoundMeatStop();//고기 소리 재생멈춤 
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            MeatSoundOnOffSign();
            if (other.gameObject.CompareTag("MEAT")) //고기가 닿으면 
            {
                haveMeat = true;
            }
            else if (other != null)
                haveMeat = false;
        }


    }
}




