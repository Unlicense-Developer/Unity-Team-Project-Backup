using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GrillingMeatGame
{
    public class GameManager : MonoBehaviour
    {
        //싱글톤
        public static GameManager instance;

        ParticleManager particleManager;
        UI ui;
        ScoreManager scoreManager;
        SoundManager soundManager;

        [SerializeField]
        int scorePoint;
        //액션 +스코어
        #region 사운드
        public Action ScoreAdd;
        public Action ScoreMinus;

        public Action SoundMeatPlay;
        public Action SoundMeatUnPause;
        public Action SoundMeatStop;
        public Action SoundEatPlay;
        public Action SoundMeatLoop;
        public Action SoundMeatStopLoop;
        public Action SoundMute;
        #endregion

        public Action OnSmokeParticle;
        public Action OffSmokeParticle;

        //액션 - 스코어

        void Awake()
        {
            if (null == instance)
                instance = this;
        }

        void Start()
        {
            ui = GameObject.Find("InGameUICanvas").GetComponent<UI>();
            soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
            scoreManager = gameObject.AddComponent<ScoreManager>();
            particleManager = GameObject.Find("ParticleManager").GetComponent<ParticleManager>();
            BindEvents();

        }

        public void BindEvents()
        {
            //연기 파티클
            this.OnSmokeParticle += particleManager.OnLargeSmoke;
            this.OffSmokeParticle += particleManager.OffAllSmoke;

            //먹는 소리 재생
            this.SoundEatPlay += soundManager.PlayEatSound;
            //고기 소리 재생 
            this.SoundMeatPlay += soundManager.PlayMeatGrillSound;
            //고기 소리 다시 재생
            this.SoundMeatUnPause += soundManager.UnPauseMeatGrillSound;
            //고기 소리멈춤 
            this.SoundMeatStop += soundManager.StopMeatGrillSound;

            //고기 소리 반복
            this.SoundMeatLoop += soundManager.PlayloopGrillSound;
            //고기 소리 반복멈춤
            this.SoundMeatStopLoop += soundManager.StoploopGrillSound;
            //소리 음소거
            this.SoundMute += soundManager.SoundMute;

            //점수 계산
            this.ScoreAdd += scoreManager.AddScore;
            this.ScoreMinus += scoreManager.MinusScore;

            //점수 UI 출력
            scoreManager.ScoreAddChangeNow += ui.NowScoreDisplay;
            scoreManager.ScoreMinusChangeNow += ui.NowScoreDisplay;
        }



        public void UnBindEvents()
        {
            //연기 파티클
            this.OnSmokeParticle -= particleManager.OnLargeSmoke;
            this.OffSmokeParticle -= particleManager.OffAllSmoke;

            //먹는 소리 재생
            this.SoundEatPlay -= soundManager.PlayEatSound;
            //고기 소리 재생 
            this.SoundMeatPlay -= soundManager.PlayMeatGrillSound;
            //고기 소리 다시 재생
            this.SoundMeatUnPause -= soundManager.UnPauseMeatGrillSound;
            //고기 소리멈춤 
            this.SoundMeatStop -= soundManager.StopMeatGrillSound;

            //고기 소리 반복
            this.SoundMeatLoop -= soundManager.PlayloopGrillSound;
            //고기 소리 반복멈춤
            this.SoundMeatStopLoop -= soundManager.StoploopGrillSound;
            //소리 음소거
            this.SoundMute -= soundManager.SoundMute;

            //점수 계산
            this.ScoreAdd -= scoreManager.AddScore;
            this.ScoreMinus -= scoreManager.MinusScore;

            //점수 UI 출력
            scoreManager.ScoreAddChangeNow -= ui.NowScoreDisplay;
            scoreManager.ScoreMinusChangeNow -= ui.NowScoreDisplay;
        }

        private void OnDestroy()
        {
            UnBindEvents();
        }


        void Update()
        {


        }

    }
}












