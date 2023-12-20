using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �Ǽ��� ������Ʈ�� �����ϴ� ���� �����ϱ� ���� ����
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMoveController : MonoBehaviour
{
    PlayerController.ThirdPersonController Controller;
    Animator anim; // �ִϸ�����

    private bool canFishA; // ���� ���� ���� �Ǵ��� ���� ����
    private bool canFishB;

    public float timerToCatch; // ���� �ð�
    public float timeBeforeBite; // ����� ����� �ð�
    public bool isFishing; // �÷��̾ ���� �� ���� �Ǵ��� ���� ����

    bool functionTriggered = false; //���� �ִϸ��̼� ������ ���� �� ������� ���� ����

    public GameObject fishingZone_1; // ���� ����1
    public GameObject fishingZone_2; // ���� ����2

    public GameObject fishingPole; // ���˴� ���� ������Ʈ

    public bool isFishingActive = true; //���ý��� �� ������� ���� ����

    FishingZone_ ZoneManager;
    HitQTE qte;
    MiniGame Minigame;

    private string zonTag = "";
    void Start()
    {
        //player = GetComponent<Rigidbody>(); // ������ٵ� ������Ʈ �Ҵ�
        anim = GetComponent<Animator>(); // �ִϸ����� ������Ʈ �Ҵ�.
        Controller = GetComponent<PlayerController.ThirdPersonController>();

        fishingPole.SetActive(false); // ���˴� ��Ȱ��ȭ
    }

    void LateUpdate()
    {
        Minigame.fishTimer -= Time.deltaTime;
        timerToCatch += Time.deltaTime;

        // Ű�� ������ ���� �õ�
        if (!functionTriggered && Input.GetKeyDown(KeyCode.K))
        {
            functionTriggered = true;   //���۽���
            fishingPole.SetActive(true);
            FishCheck();
            functionTriggered = false;  //���� ������� ���� ����
        }

        // �̵� �� ���� �ߴ�
        if (Controller._isMoving == true && isFishing)
        {
            Debug.Log("�������� ���ð� ��ҵǾ����ϴ�.");
            isFishing = false;
            fishingPole.SetActive(false);
        }

        // �������� �ð��� ������ ����Ⱑ �̳��� ����
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
        canFishA = fishingZone_1.GetComponent<FishingZone_>().PlayerCanFish(); //���ð� �������� üũ
        canFishB = fishingZone_2.GetComponent<FishingZone_>().PlayerCanFish();

        if (canFishA || canFishB)
        {
            Fish(); // ���� ����
            Debug.Log("���� ���Դϴ�!");
        }
        else
        {
            Debug.Log("�̰������� ������ �� �����ϴ�!");
        }
    }

    void Fish()
    {
        timerToCatch = 0f;
        timeBeforeBite = 0f;
        System.Random random = new System.Random();

        // ����Ⱑ �̳��� ���µ� �ɸ��� ������ �ð� ����  (0~10�� + 5��)
        timeBeforeBite = random.Next(10) + 5;

        //���õ����� ����
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
