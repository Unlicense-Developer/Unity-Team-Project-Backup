using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitQTE : MonoBehaviour
{
    //연타형 QTE 관련 변수
    PlayerController.ThirdPersonController Controller;
    PlayerFishingController FC;
    public int requiredPresses = 30; // 필요한 연타 횟수
    public Slider HitqteSlider; // 연타 슬라이더
    public GameObject qtePanel; // QTE 패널
    private int currentPresses = 0; // 현재까지의 연타 횟수
    private float qteTimer = 10.0f; // QTE 제한 시간
    private bool isHitQTEActive = true; // QTE 활성화 상태
    Animator anim;

    private void Start()
    {
        Controller = GetComponent<PlayerController.ThirdPersonController>();
        FC = GetComponent<PlayerFishingController>();
    }

    public void HitQTEAction()
    {

        anim.SetBool("ReelIn", true);  //릴을 감는 애니메이션을 동작
        qtePanel.SetActive(true);   //QTE페널 활성화
        HitqteSlider.gameObject.SetActive(true);    //QTE슬라이더 활성화

        if (FC.isFishingActive)
        {
            // J 키를 눌렀을 때 QTE의 성공도를 상승
            if (Input.GetKeyDown(KeyCode.J))
            {
                anim.SetBool("ReelIn", true);
                currentPresses++; // 입력될 연타 횟수 증가
                Debug.Log(currentPresses);
                HitqteSlider.value = (float)currentPresses / requiredPresses; // 연타 횟수에 비례하여 슬라이더 값 조절
                print("c : " + currentPresses + " r : " + requiredPresses);
                // 필요한 연타 횟수에 도달하면 성공
                if (currentPresses >= requiredPresses)
                {
                    anim.SetBool("ReelIn", false);
                    anim.SetInteger("Catch", 0);
                    hideHitQTEPanel(); // QTE 성공 후 패널 숨김
                    ResetHitQTE(); // QTE 초기화
                    Debug.Log("물고기를 잡았습니다!");
                    FC.isFishing = false;
                    anim.SetBool("IsFishing", false);
                    Invoke("DisableFishingPole", 2.0f);
                }
            }

            // 시간의 감소와 만료했을때 실패하고 그 이후 동작을 설정
            qteTimer -= Time.deltaTime;
            if (qteTimer <= 0)
            {
                Debug.Log("시간 초과 - QTE 실패!");
                ResetHitQTE(); // 시간 초과 시 QTE 초기화
                hideHitQTEPanel();
                hideSlider();
                anim.SetBool("IsFishing", false);
                anim.SetBool("ReelIn", false);
                anim.SetInteger("Catch", 1);
                FC.isFishing = false;
                Invoke("DisableFishingPole", 2.0f);
            }
        }
    }

    void hideHitQTEPanel()
    {
        // QTE 패널 비활성화 및 1초 후에 슬라이더 숨김
        qtePanel.SetActive(false);
        Invoke("hideSlider", 1.0f);
    }

    void hideSlider()
    {
        // 슬라이더 숨기기
        HitqteSlider.gameObject.SetActive(false);
    }

    void ResetHitQTE()
    {
        // QTE에 입력된 값을 초기화하고 다음 도전을 준비
        HitqteSlider.value = 0;
        currentPresses = 0;
        qteTimer = 10.0f;
        FC.timerToCatch = 0;
        FC.isFishingActive = false; // QTE 비활성화
    }


}
