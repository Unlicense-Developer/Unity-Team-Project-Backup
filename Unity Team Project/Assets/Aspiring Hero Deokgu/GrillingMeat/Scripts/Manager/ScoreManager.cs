using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GrillingMeatGame
{
    public class ScoreManager : MonoBehaviour
    {

        readonly int scorePoint;
        int score;
        Animator meatColorChange;

        public Action<int> ScoreAddChangeNow; // 점수 + 액션 이벤트
        public Action<int> ScoreMinusChangeNow; // 점수 - 액션 이벤트

        public ScoreManager(int scorePoint)
        {
            this.scorePoint = scorePoint;

        }
        void Start()
        {
            meatColorChange = GameObject.FindWithTag("MEAT").GetComponent<Animator>();

        }
        //점수 계산
        public void AddScore()
        {
            //애니메이션 재생 시간이 ~ 일때 
            score += 60; //60
            ScoreAddChangeNow?.Invoke(score);
            //0~1 (1이면 100% 재생이 끝난상태)
        }
        public void MinusScore()
        {
            //애니메이션 재생 시간이 ~ 일때
            score -= 10;
            ScoreMinusChangeNow?.Invoke(score);
        }

        void Update()
        {

        }

    }
}











