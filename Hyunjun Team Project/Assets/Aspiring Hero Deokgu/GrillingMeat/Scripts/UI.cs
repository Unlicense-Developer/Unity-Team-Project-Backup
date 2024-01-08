using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace GrillingMeatGame
{
    public class UI : MonoBehaviour
    {
        public static UI instance;
        float limitTime = 60.0f; //300초
        [SerializeField]
        TMP_Text scoreText;

        [SerializeField]
        TMP_Text timeText;

        [SerializeField]
        Slider timeSlider; //Value값 조절해서 시간 줄이기

        //점수에 따라서 [동별, 은별, 금별] SetActive(true)로 켜서 이미지 바꾸기
        [SerializeField]
        Image brownStar;
        [SerializeField]
        Image silverStar;
        [SerializeField]
        Image goldStar;

        //하트 5개 배열? 리스트? 선언후 MeatColor.MeatAniPlayingScoreStep()에서 
        //else if 에 하트 1개씩 빼는 동작 추가
        [SerializeField]
        Image heartLife;
        float maxHeart = 100f;
        float heart; //기준 숫자
        Animator meatColorChange; //고기 익는 애니메이션

        [SerializeField]
        GameObject gameOverPanel; //게임 오버 패널 
        [SerializeField]
        GameObject timeOverPanel; //타임 오버 패널 
        [SerializeField]
        GameObject missionClearPanel;
        void Awake()
        {
            if (null == instance)
                instance = this;

        }
        void Start()
        {
            heart = maxHeart;
            Time.timeScale = 1.0f;//게임시간 이 흐르게 
                                  //상태에 따라 변경하는 패널들 처음에 끄기
            gameOverPanel.SetActive(false);
            timeOverPanel.SetActive(false);
            missionClearPanel.SetActive(false);
            meatColorChange = GameObject.FindWithTag("MEAT").GetComponent<Animator>();//고기 애니 
            timeSlider.interactable = false;//타임슬라이더 멈춤 
            brownStar.enabled = true; //처음 점수에는 갈색별 보이게 
                                      //UnityEditor.EditorApplication.isPaused = false;//게임 일시정지 끄기
        }


        void HeartBar()
        {
            heartLife.fillAmount = heart / maxHeart;
        }


        public void HideHearts()//하트 깎임
        {
            heart -= 20f; //1칸씩 깎이게 
            if (heart <= 0f) //하트가 0이면 
            {
                GameOverSign();//게임오버
            }
        }



        public void GameOverSign() //게임 오버 상태
        {
            GameManager.instance.SoundMute.Invoke(); //고기 소리 멈춤
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f;
        }


        void HideStars() //별들 안보이게
        {
            silverStar.enabled = false;
            goldStar.enabled = false;
        }

        void TimerSlider() //제한시간 바 이미지 
        {
            if (timeSlider.value > 0.0f)
            {
                timeSlider.value -= Time.deltaTime; //시간이 흐를수록 타임 슬라이더도 줄어듬
            }
        }


        void NowLimitTime() //제한시간 숫자
        {
            limitTime -= Time.deltaTime; //제한시간 흘러가게
            timeText.text = Mathf.Round(limitTime).ToString(); //제한시간 글씨로 변경
                                                               //timeText.text = "Time :  " + Mathf.Round(limitTime);
            TimeZero();
        }
        void TimeZero()
        {
            //시간초 0이 이하로 안떨어진다
            if (limitTime < 0)
            {
                limitTime = 0f; // 방어코드 
                TimeOverSign(); //타임오버 패널 켜기
            }
        }
        void TimeOverSign() //타임오버 상태
        {
            GameManager.instance.SoundMute.Invoke(); //고기 소리 멈춤
            timeOverPanel.SetActive(true); //타임오버 패널 ON
            Time.timeScale = 0f; //게임 시간 멈춤
        }

        //점수 UI 연결 
        public void NowScoreDisplay(int score) //현재 점수
        {
            ViewStars(score);
            scoreText.text = score.ToString(); //점수를 숫자로 변환

        }

        void ViewStars(int score) //점수에 따라서 별색상 변경
        {
            //점수 대략 정하기
            if (score >= 480)
            {
                silverStar.enabled = true; //실버
                brownStar.enabled = false;
            }
            if (score >= 900)
            {
                goldStar.enabled = true; //골드 
                silverStar.enabled = false;
                MissionClear();
            }
        }
        void MissionClear()
        {
            missionClearPanel.SetActive(true);
        }
        void Update()
        {
            NowLimitTime();//제한시간 숫자
            TimerSlider();//제한시간 바
            HeartBar();//하트 줄어듬
        }
    }
}


















