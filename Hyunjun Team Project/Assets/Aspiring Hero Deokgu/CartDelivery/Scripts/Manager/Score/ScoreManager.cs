using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartDelivery
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;//싱글톤
        //초기 고정값(상품값어치)
        public int productPrice = 1000;

        //상품이 떨어질때마다 50~100 사이 값 -줌
        void Awake()
        {
            if (null == instance)
                instance = this;
        }
        void Start()
        {

        }
        public void MinusProductPrice(int Price) //물건 값어치 빼기
        {
            //시작 값 = 1000
            productPrice -= Price;
        }

        void Update()
        {

        }
    }
}