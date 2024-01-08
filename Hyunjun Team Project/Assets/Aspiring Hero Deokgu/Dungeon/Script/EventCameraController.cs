using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Dungeon;


public class EventCameraController : MonoBehaviour
{
    public static EventCameraController Instacne;

    public Camera mainCamera; // 원래 사용하던 카메라
    public Camera eventCamera; // 이벤트 카메라
    public Camera eventCamera_2; // 이벤트 카메라 2

    private bool isSpecialActive = false;

    private void Awake()
    {
        if (Instacne == null) Instacne = this;
    }

    private void Start()
    {
        eventCamera.gameObject.SetActive(false);
        eventCamera_2.gameObject.SetActive(false);
    }

    public void EventOn()
    {
        StartCoroutine(EvnetCamera1Effect());
        TextGUIManager.Instance.EventCameraTextA();
    }

    public void OtherEvnetOn()
    {
        StartCoroutine(EvnetCamera2Effect());
        TextGUIManager.Instance.EventCameraTextB();
    }


    IEnumerator EvnetCamera1Effect()
    {
        // 움직임 막기
        // 예시: PlayerController 스크립트에서 플레이어의 움직임을 막을 수 있습니다.
        // playerController.DisableMovement();

        if (!isSpecialActive)
        {
            isSpecialActive = true;

            // 특정 카메라 활성화
            eventCamera.gameObject.SetActive(true);

            // DOTween을 사용하여 특정 카메라에 흔들림 효과 주기
            eventCamera.transform.DOShakePosition(10f, strength: 0.2f, vibrato: 13, randomness: 90);

            yield return new WaitForSeconds(12f);

            mainCamera.gameObject.SetActive(true);      // 본래 사용하던 카메라로 복귀
            eventCamera.gameObject.SetActive(false);    //이벤트 카메라 비활성화

            isSpecialActive = false;


            // 움직임 다시 허용
            // 예시: playerController.EnableMovement();
        }
    }

    IEnumerator EvnetCamera2Effect()
    {
        // 움직임 막기
        // 예시: PlayerController 스크립트에서 플레이어의 움직임을 막을 수 있습니다.
        // playerController.DisableMovement();

        if (!isSpecialActive)
        {
            isSpecialActive = true;

            // 특정 카메라 활성화
            eventCamera_2.gameObject.SetActive(true);

            // DOTween을 사용하여 특정 카메라에 흔들림 효과 주기
            eventCamera_2.transform.DOShakePosition(10f, strength: 0.1f, vibrato: 10, randomness: 90);

            yield return new WaitForSeconds(12f);

            mainCamera.gameObject.SetActive(true);      // 본래 사용하던 카메라로 복귀
            eventCamera_2.gameObject.SetActive(false);    //이벤트 카메라 비활성화

            isSpecialActive = false;

            // 움직임 다시 허용
            // 예시: playerController.EnableMovement();
        }
    }
}