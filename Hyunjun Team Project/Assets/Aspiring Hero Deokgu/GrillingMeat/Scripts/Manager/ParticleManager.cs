using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrillingMeatGame
{
    public class ParticleManager : MonoBehaviour
    {
        public static ParticleManager instance;
        #region 파티클
        [SerializeField]
        GameObject smokeLarge; //연기 파티클(대)
                               //ParticleSystem smokeLarge;
        [SerializeField]
        GameObject smokeSmallOne; //연기 파티클(소)1
                                  //ParticleSystem smokeSmallOne;

        #endregion

        void Awake()
        {
            if (null == instance)
                instance = this;
        }
        void Start()
        {
            OnSmallSmoke();
            OffAllSmoke();
        }

        public void OffAllSmoke()
        {
            smokeLarge.SetActive(false);
        }

        void OnSmallSmoke()
        {
            smokeSmallOne.SetActive(true);
        }

        public void OnLargeSmoke()
        {
            smokeLarge.SetActive(true);
        }
        void Update()
        {

        }
    }
}