using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrillingMeatGame
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        AudioSource audioSource;

        //게임 상태 소리 
        [SerializeField]
        AudioClip gameClearSound; //게임 클리어
        [SerializeField]
        AudioClip gameOverSound; // 게임 오버

        //게임 UI 소리 
        [SerializeField]
        AudioClip optionSound; // 옵션 소리 
        [SerializeField]
        AudioClip timeSound; //시계 돌아가는 소리
        [SerializeField]
        AudioClip timeOverSound; //시간 초과 소리

        //고기 관련 소리 
        [SerializeField]
        AudioClip meatGrillSound; //고기 익는 소리 
        [SerializeField]
        AudioClip eatSound; //고기 먹는 소리 

        void Awake()
        {

        }



        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.mute = false;
            audioSource.Stop();
        }

        #region 고기 굽는소리 
        public void PlayMeatGrillSound()
        {
            audioSource.mute = false;
            audioSource.clip = meatGrillSound;
            audioSource.loop = true;
            audioSource.Play();

        }

        public void StopMeatGrillSound()
        {
            audioSource.loop = false;
            audioSource.Stop();
        }

        public void UnPauseMeatGrillSound()
        {
            audioSource.loop = true;
            audioSource.UnPause();
        }

        public void PlayloopGrillSound()
        {
            audioSource.loop = true;
        }

        public void StoploopGrillSound()
        {
            audioSource.loop = false;
        }
        #endregion

        public void SoundMute()
        {
            audioSource.mute = true;
        }
        public void PlayEatSound()
        {
            audioSource.PlayOneShot(eatSound); //재생
        }
    }
}










