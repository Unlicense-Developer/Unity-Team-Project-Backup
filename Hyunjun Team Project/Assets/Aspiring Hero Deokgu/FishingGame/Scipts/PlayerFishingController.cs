using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFishingController : MonoBehaviour
{
    PlayerController.ThirdPersonController Controller;
    Animator anim; // 애니메이터

    private bool canFishA; // 낚시 가능 여부 판단을 위한 변수
    private bool canFishB;

    public int FishingResult;
    public float timerToCatch; // 낚시 시간
    public float timeBeforeBite; // 물고기 물어듦 시간
    public bool isFishing; // 플레이어가 낚시 중 인지 판단을 위한 변수

    bool functionTriggered = false; //낚시 애니메이션 동작의 실행 및 재실행을 위한 변수

    public GameObject fishingZone_1; // 낚시 지점1
    public GameObject fishingZone_2; // 낚시 지점2

    public GameObject fishingPole; // 낚싯대 게임 오브젝트

    public bool isFishingActive = true; //낚시실행 및 재실행을 위한 변수

    //연타형 QTE 관련 변수
    public int requiredPresses = 30; // 필요한 연타 횟수
    public Slider HitqteSlider; // 연타 슬라이더
    public GameObject qtePanel; // QTE 패널
    private int currentPresses = 0; // 현재까지의 연타 횟수
    private float qteTimer = 10.0f; // QTE 제한 시간


    //낚시 미니게임 관련 변수
    public GameObject MinigamePanel;

    [SerializeField] Transform topPivot;        //낚시 미니게임 움직임 제한을 위한 변수
    [SerializeField] Transform bottomPivot;

    [SerializeField] Transform fish;

    private float fishPosition;
    private float fishDestination;

    private float fishTimer;
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

    [SerializeField] float failTimer = 10f;

    //낚시지역 판별을 위한 변수
    private string zonTag = "";

    private bool isPlayingSound = false;

    void Start()
    {
        //player = GetComponent<Rigidbody>(); // 리지드바디 컴포넌트 할당
        anim = GetComponent<Animator>(); // 애니메이터 컴포넌트 할당.
        Controller = GetComponent<PlayerController.ThirdPersonController>();

        fishingPole.SetActive(false); // 낚싯대 비활성화
    }

    void LateUpdate()
    {
        fishTimer -= Time.deltaTime;
        timerToCatch += Time.deltaTime;

        // 키를 누르면 낚시 시도
        if (!functionTriggered && Input.GetKeyDown(KeyCode.K))
        {
            functionTriggered = true;   //동작실행
            FishCheck();
            functionTriggered = false;  //동작 재실행을 위해 리셋
        }

        // 이동 시 낚시 중단
        if (Controller._isMoving == true && isFishing)
        {
            Debug.Log("움직여서 낚시가 취소되었습니다.");
            isFishing = false;
            fishingPole.SetActive(false);
        }

        // 정해지는 시간이 지나면 물고기가 미끼를 물음
        if (timerToCatch > timeBeforeBite && isFishing && !Controller._isMoving)
        {
            //Debug.Log("Hit!!!");
            FishingZoneEntered(zonTag);
            if(!isFishingActive)
                isFishingActive = true;
        }

        else
        {
            isPlayingSound = false;
        }
    }

    IEnumerator CastSound()
    {
        yield return new WaitForSeconds(2f);
        WorldSoundManager.Instance.PlaySFX("Cast");
    }


    void DisableFishingPole()
    {
        fishingPole.SetActive(false);
    }

    void FishCheck()
    {
        canFishA = fishingZone_1.GetComponent<FishingZone_>().PlayerCanFish(); //낚시가 가능한지 체크
        canFishB = fishingZone_2.GetComponent<FishingZone_>().PlayerCanFish();

        if (canFishA || canFishB)
        {
            fishingPole.SetActive(true);
            Fish(); // 낚시 시작
            Debug.Log("낚시 중입니다!");
        }
        else
        {
            Debug.Log("이곳에서는 낚시할 수 없습니다!");
            fishingPole.SetActive(false);
        }
    }

    void Fish()
    {
        FishingResult = Random.Range(1, 3);
        Debug.Log(FishingResult);
        timerToCatch = 0f;
        timeBeforeBite = 0f;
        System.Random random = new System.Random();

        // 물고기가 미끼를 물어드는데 걸리는 랜덤한 시간 설정
        timeBeforeBite = random.Next(10) + 5;

        //낚시동작을 실행
        Debug.Log(timeBeforeBite);
        isFishing = true;
        anim.SetBool("IsFishing", true);
        StartCoroutine(CastSound());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("zone1"))
        {
            zonTag = other.gameObject.tag;
            //FishingZoneEntered("zone1");
        }
        else if (other.CompareTag("zone2"))
        {
            zonTag = other.gameObject.tag;
            // FishingZoneEntered("zone2");
        }
    }

    void FishingZoneEntered(string zoneTag)
    {
        if (zoneTag == "zone1")
        {
            HitQTE();
        }
        else if (zoneTag == "zone2")
        {
            pullingQTE();
        }
    }

    void HitQTE()
    {
        anim.SetBool("ReelIn", true);  //릴을 감는 애니메이션을 동작

        qtePanel.SetActive(true);   //QTE페널 활성화
        HitqteSlider.gameObject.SetActive(true);    //QTE슬라이더 활성화

        if (isFishingActive)
        {
            // J 키를 눌렀을 때 QTE의 성공도를 상승
            if (Input.GetKeyDown(KeyCode.J))
            {
                anim.SetBool("ReelIn", true);
                currentPresses++;
                HitqteSlider.value = (float)currentPresses / requiredPresses; // 연타 횟수에 비례하여 슬라이더 값 조절
                //print("c : " + currentPresses + " r : " + requiredPresses);
                // 필요한 연타 횟수에 도달하면 성공
                if (currentPresses >= requiredPresses)
                {
                    anim.SetBool("ReelIn", false);
                    anim.SetInteger("Catch", 0);
                    StartCoroutine(SuccessSound());
                    hideHitQTEPanel(); // QTE 성공 후 패널 숨김
                    ResetHitQTE(); // QTE 초기화
                    Debug.Log("물고기를 잡았습니다!");
                    isFishing = false;
                    anim.SetBool("IsFishing", false);
                    Invoke("DisableFishingPole", 2.0f);

                    if (FishingResult <= 1)
                    {
                        InventoryManager.Instance.AddItem("Bass");
                    }
                    else
                    {
                        InventoryManager.Instance.AddItem("Trout");
                    }
                }
            }

            // 시간의 감소와 만료했을때 실패하고 그 이후 동작을 설정
            qteTimer -= Time.deltaTime;
            if (qteTimer <= 0)
            {
                Debug.Log("시간 초과 - QTE 실패!");
                StartCoroutine(FailSound());
                ResetHitQTE(); // 시간 초과 시 QTE 초기화
                hideHitQTEPanel();
                hideSlider();
                anim.SetBool("IsFishing", false);
                anim.SetBool("ReelIn", false);
                anim.SetInteger("Catch", 1);
                isFishing = false;
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
        timerToCatch = 0;
        isFishingActive = false; // QTE 비활성화
    }

    void pullingQTE()   //낚시 미니게임에 관련된 함수
    {

        anim.SetBool("ReelIn", true);

        if (isFishingActive)
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
        StartCoroutine(SuccessSound());
        anim.SetBool("ReelIn", false); ;
        anim.SetInteger("Catch", 0);
        Debug.Log("물고기를 잡았다!");
        ResetMinigame();
        hideMinigame();
        isFishing = false;
        Invoke("DisableFishingPole", 2.0f);

        if (FishingResult <= 1)
        {
            InventoryManager.Instance.AddItem("Salmon");
        }
        else
        {
            InventoryManager.Instance.AddItem("Tuna");
        }
    }

    void LoseFish()
    {
        anim.SetBool("ReelIn", false); ;
        anim.SetInteger("Catch", 1);
        StartCoroutine(FailSound());
        Debug.Log("물고기를 놓쳤다!");
        ResetMinigame(); // 시간 초과 시 QTE 초기화
        hideMinigame();
        isFishing = false;
        Invoke("DisableFishingPole", 2.0f);
    }

    void ResetMinigame()
    {
        //미니게임 입력된 값 초기화
        fishPosition = 0.1f;
        hookPullVelocity = 0f;
        hookProgress = 0;
        hookPosition = 0.1f;
        timerToCatch = 0;
        anim.SetBool("IsFishing", false);
        isFishingActive = false;
    }

    void hideMinigame()
    {
        //미니게임 페널 숨김
        MinigamePanel.SetActive(false);
    }

    IEnumerator SuccessSound()
    {
        yield return new WaitForSeconds(0.8f);
        WorldSoundManager.Instance.PlaySFX("Catch");
        yield return new WaitForSeconds(1.5f);
        WorldSoundManager.Instance.PlaySFX("Success");
    }

    IEnumerator FailSound()
    {
        yield return new WaitForSeconds(2f);
        WorldSoundManager.Instance.PlaySFX("Fail");
    }

    void PlayReelIn()
    {
        WorldSoundManager.Instance.PlaySFX("ReelIn");
    }

}