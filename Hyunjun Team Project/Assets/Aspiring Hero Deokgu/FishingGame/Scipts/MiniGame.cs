using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    //낚시 미니게임 관련 변수
    PlayerController.ThirdPersonController Controller;
    PlayerFishingController FC;
    Animator anim;
    public GameObject MinigamePanel;

    [SerializeField] Transform topPivot;        //낚시 미니게임 움직임 제한을 위한 변수
    [SerializeField] Transform bottomPivot;

    [SerializeField] Transform fish;

    private float fishPosition;
    private float fishDestination;

    public float fishTimer;
    [SerializeField] float timerMultiplicator = 3f; //물고기의 랜덤한 움직임을 위한 변수 

    private float fishSpeed;
    [SerializeField] float smoothMotion = 1f;       //물고기의 움직임을 부드럽게

    [SerializeField] Transform hook;
    [SerializeField] float hookPosition;

    [SerializeField] float hookSize = 0.4f;         //미니게임 hookArea의 크기를 정하는 변수
    [SerializeField] float hookPower = 5f;          //hookArea의 상승을 위한 hookPullVelocity에 관련된 변수
    private float hookProgress;
    [SerializeField] float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;   //hookArea의 상승을 위한 변수
    [SerializeField] float hookGravityPower = 0.005f;   //hookArea의 하강을 위한 변수
    [SerializeField] float hookProgressDegradationPower = 0.1f; //미니게임 성공률 게이지 상승 관련 변수

    [SerializeField] Transform progressBarContainer;

    public bool fishingfalse = false;

    [SerializeField] float failTimer = 10f;

    private void Start()
    {
        Controller = GetComponent<PlayerController.ThirdPersonController>();
        FC = GetComponent<PlayerFishingController>();
    }

    public void pullingQTE()   //낚시 미니게임에 관련된 함수
    {
        anim.SetBool("ReelIn", true);

        if (FC.isFishingActive)
        {
            MinigamePanel.SetActive(true);
            fishing();
            Hook();
            ProgressCheck();
        }
    }

    void fishing()
    {   //미니게임에서 물고기가 랜덤한 위치로 움직이게 하기위한 함수
        if (fishTimer < 0f)
        {
            fishTimer = UnityEngine.Random.value * timerMultiplicator;

            fishDestination = UnityEngine.Random.value;
        }
        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }

    void Hook()
    {   //미니게임에서 훅의 움직임을 위한 함수 (훅의 속도와 움직이는 위치를 제한함)
        if (Input.GetMouseButton(0))
        {
            hookPullVelocity += hookPullPower * Time.deltaTime;
        }
        hookPullVelocity -= hookGravityPower * Time.deltaTime;
        hookPullVelocity = Mathf.Clamp(hookPullVelocity, -0.02f, 0.02f);

        hookPosition += hookPullVelocity;
        hookPosition = Mathf.Clamp(hookPosition, 0.1f, 0.9f);
        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }

    void ProgressCheck()
    {   //미니게임의 성공도를 상승,하강시키기 위한 함수
        Vector3 Is = progressBarContainer.localScale;
        Is.y = hookProgress;
        progressBarContainer.localScale = Is;

        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if (min < fishPosition && fishPosition < max)
        {
            hookProgress += hookPower * Time.deltaTime;
        }
        else
        {
            hookProgress -= hookProgressDegradationPower * Time.deltaTime;

            failTimer -= Time.deltaTime;
            if (failTimer <= 0f)
            {
                LoseFish();
            }
        }

        if (hookProgress >= 1f)
        {
            Catch();
        }

        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);
    }

    void Catch()
    {
        anim.SetBool("ReelIn", false); ;
        anim.SetInteger("Catch", 0);
        Debug.Log("물고기를 잡았다!");
        ResetMinigame();
        hideMinigame();
        FC.isFishing = false;
        Invoke("DisableFishingPole", 2.0f);
    }

    void LoseFish()
    {
        anim.SetBool("ReelIn", false); ;
        anim.SetInteger("Catch", 1);
        Debug.Log("물고기를 놓쳤다!");
        ResetMinigame(); // 시간 초과 시 QTE 초기화
        hideMinigame();
        FC.isFishing = false;
        Invoke("DisableFishingPole", 2.0f);
    }

    void ResetMinigame()
    {
        //미니게임 입력된 값 초기화
        fishPosition = 0.1f;
        hookPullVelocity = 0f;
        hookProgress = 0;
        hookPosition = 0.1f;
        FC.timerToCatch = 0;
        anim.SetBool("IsFishing", false);
        FC.isFishingActive = false;
    }

    void hideMinigame()
    {
        //미니게임 페널 숨김
        MinigamePanel.SetActive(false);
    }
}
