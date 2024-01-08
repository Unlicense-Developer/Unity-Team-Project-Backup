using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CartDelivery
{
    public class ProductsTriggerSign : MonoBehaviour
    {

        #region 물건들 값 태그(크기별)
        const string large = "Large";
        const string mediumlarge = "MediumLarge";
        const string medium = "Medium";
        const string smallmedium = "SmallMedium";
        const string small = "Small";
        #endregion

        #region 물건들 값
        public int largePrice = 100; //대
        public int mediumlargePrice = 70; //중+대
        public int mediumPrice = 50; //중
        public int smallmediumPrice = 30; //소+중
        public int smallPrice = 10; //소
        #endregion

        void Start()
        {

        }

        private void OnCollisionEnter(Collision other) //무슨 식품, 물건인가에 따라서 점수가 깎인다
        {
            if (other.gameObject.CompareTag("Floor"))
            {
                switch (this.gameObject.tag)
                {
                    case large:
                        ScoreManager.instance.MinusProductPrice(largePrice);
                        break;
                    case mediumlarge:
                        ScoreManager.instance.MinusProductPrice(mediumlargePrice);
                        break;
                    case medium:
                        ScoreManager.instance.MinusProductPrice(mediumPrice);
                        break;
                    case smallmedium:
                        ScoreManager.instance.MinusProductPrice(smallmediumPrice);
                        break;
                    case small:
                        ScoreManager.instance.MinusProductPrice(smallPrice);
                        break;
                }
            }
            return;

        }
        void Update()
        {

        }
    }
}


