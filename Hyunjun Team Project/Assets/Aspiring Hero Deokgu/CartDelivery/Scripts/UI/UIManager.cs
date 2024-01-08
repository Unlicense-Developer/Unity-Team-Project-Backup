using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CartDelivery
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        const string productPriceText = "Product Price : "; //UI 앞에 첫 글자
        [SerializeField]
        TMP_Text productPriceTmp;//물건 값어치 글자

        //등급 별 이미지(동별,은별,금별)
        [SerializeField]
        GameObject scoreStarBrown;
        [SerializeField]
        GameObject scoreStarSliver;
        [SerializeField]
        GameObject scoreStarGold;


        [SerializeField]
        GameObject gameStartPanel;//스타트창
        [SerializeField]
        GameObject clearPanel;//클리어창
        [SerializeField]
        GameObject gameOverPanel;//게임오버창
        [SerializeField]
        GameObject gameStartCountPanel;//게임스타트 카운트창
        [SerializeField]
        TMP_Text startCountTmp;//게임카운트 텍스트
        [SerializeField]
        GameObject gameReadyPanel;//게임레디 텍스트
        [SerializeField]
        GameObject gameGoPanel;//게임고 패널
        [SerializeField]
        GameObject KeyGuideUIPanel;//게임키 가이드창
        bool activeKeyGuide = false;
        float countDownTime = 3.0f;
        // [SerializeField]
        void Awake()
        {
            if (null == instance)
                instance = this;
        }
        void Start()
        {
            //Time.timeScale = 1f;
            OffStart();
        }

        void OffStart() //시작할때 끄는것들
        {

            //gameStartPanel.SetActive(true);
            gameStartCountPanel.SetActive(true);//게임 카운트창 켜기
            gameReadyPanel.SetActive(true);//게임레디창 켜기
            gameGoPanel.SetActive(false);//게임go이미지창 끄기
            clearPanel.SetActive(false);//게임클리어창 끄기
            gameOverPanel.SetActive(false);//게임오버창 끄기
            KeyGuideUIPanel.SetActive(false);
            //등급별 이미지 끄기 
            scoreStarBrown.SetActive(false);//동별
            scoreStarSliver.SetActive(false);//은별
            scoreStarGold.SetActive(false);//금별
        }
        public void ScoreStarChangeSign()//스코어에 따라서 다른색 별 켜기
        {
            int ProdutPrice = ScoreManager.instance.productPrice;
            if (ProdutPrice >= 1000) //1000점이상 
            {
                scoreStarGold.SetActive(true);//금별
            }
            else if (ProdutPrice >= 500)//500점이상 
            {
                scoreStarSliver.SetActive(true);//은별
            }
            else//그외 == 0점이상~499점이하
            {
                scoreStarBrown.SetActive(true);//동별
            }
        }

        void UIproductPrice()//TMP_Text productPrice 글씨를  ScoreManager.MinusProductPrice에 있는 productPrice값을 보여준다
        {
            int ProdutPrice = ScoreManager.instance.productPrice;
            productPriceTmp.text = productPriceText + Convert.ToString(ProdutPrice);
        }
        void UIStartCount()
        {
            countDownTime -= Time.deltaTime;
            startCountTmp.text = Mathf.Round(countDownTime).ToString();
            if (countDownTime < 1)
            {
                countDownTime = 1f;
                gameReadyPanel.SetActive(false);//게임레디창 켜기
                gameGoPanel.SetActive(true);//게임go이미지 켜기
                GameStartSign();
            }
            //startCountTmp.text = startCountTmp.ToString()
        }
        void GameStartSign()
        {
            gameGoPanel.SetActive(false);//게임go이미지 끄기
        }
        public void OnGameOver()
        {
            gameOverPanel.SetActive(true);
        }
        public void OnGameClear()
        {
            clearPanel.SetActive(true);
        }
        public void OnKeyGuideUIButton()
        {
            KeyGuideUIPanel.SetActive(true);
        }
        public void OffKeyGuideUIButton()
        {
            KeyGuideUIPanel.SetActive(false);
        }
        public void OnKeyGuideUIKey()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                activeKeyGuide = !activeKeyGuide;
                KeyGuideUIPanel.SetActive(activeKeyGuide);
            }
        }
                

        void Update()
        {
            UIproductPrice();
            ScoreStarChangeSign();
            UIStartCount();
            OnKeyGuideUIKey();
        }
    }
}

