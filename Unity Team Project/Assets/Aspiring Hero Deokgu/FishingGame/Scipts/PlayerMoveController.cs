using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 실수로 컴포넌트를 삭제하는 것을 방지하기 위한 설정
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMoveController : MonoBehaviour
{
    PlayerController.ThirdPersonController Controller;
    Animator anim; // 애니메이터

    private bool canFishA; // 낚시 가능 여부 판단을 위한 변수
    private bool canFishB;

    public float timerToCatch; // 낚시 시간
    public float timeBeforeBite; // 물고기 물어듦 시간
    public bool isFishing; // 플레이어가 낚시 중 인지 판단을 위한 변수

    bool functionTriggered = false; //낚시 애니메이션 동작의 실행 및 재실행을 위한 변수

    public GameObject fishingZone_1; // 낚시 지점1
    public GameObject fishingZone_2; // 낚시 지점2

    public GameObject fishingPole; // 낚싯대 게임 오브젝트

    public bool isFishingActive = true; //낚시실행 및 재실행을 위한 변수

    FishingZone_ ZoneManager;
    HitQTE qte;
    MiniGame Minigame;

    private string zonTag = "";
    void Start()
    {
        //player = GetComponent<Rigidbody>(); // 리지드바디 컴포넌트 할당
        anim = GetComponent<Animator>(); // 애니메이터 컴포넌트 할당.
        Controller = GetComponent<PlayerController.ThirdPersonController>();

        fishingPole.SetActive(false); // 낚싯대 비활성화
    }

    void LateUpdate()
    {
        Minigame.fishTimer -= Time.deltaTime;
        timerToCatch += Time.deltaTime;

        // 키를 누르면 낚시 시도
        if (!functionTriggered && Input.GetKeyDown(KeyCode.K))
        {
            functionTriggered = true;   //동작실행
            fishingPole.SetActive(true);
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
            Debug.Log("Hit!!!");
            FishingZoneEntered(zonTag);
            if (!isFishingActive)
                isFishingActive = true;

        }
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
            Fish(); // 낚시 시작
            Debug.Log("낚시 중입니다!");
        }
        else
        {
            Debug.Log("이곳에서는 낚시할 수 없습니다!");
        }
    }

    void Fish()
    {
        timerToCatch = 0f;
        timeBeforeBite = 0f;
        System.Random random = new System.Random();

        // 물고기가 미끼를 무는데 걸리는 랜덤한 시간 설정  (0~10초 + 5초)
        timeBeforeBite = random.Next(10) + 5;

        //낚시동작을 실행
        Debug.Log(timeBeforeBite);
        isFishing = true;
        anim.SetBool("IsFishing", true);
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
            qte.HitQTEAction();
        }
        else if (zoneTag == "zone2")
        {
            Minigame.pullingQTE();
        }
    }

}
