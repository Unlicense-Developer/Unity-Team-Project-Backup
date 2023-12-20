using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �Ǽ��� ������Ʈ�� �����ϴ� ���� �����ϱ� ���� ����
/*
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
*/
public class PlayerFishingController : MonoBehaviour
{
    /*
    Rigidbody player; // �÷��̾� ������ٵ�
    public float speed = 7f; // �̵� �ӵ�
    Vector3 moveDirection; // �̵� ����
    */

    PlayerController.ThirdPersonController Controller;
    AudioManager1 AudioM;
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

    //��Ÿ�� QTE ���� ����
    public int requiredPresses = 30; // �ʿ��� ��Ÿ Ƚ��
    public Slider HitqteSlider; // ��Ÿ �����̴�
    public GameObject qtePanel; // QTE �г�
    private int currentPresses = 0; // ��������� ��Ÿ Ƚ��
    private float qteTimer = 10.0f; // QTE ���� �ð�


    //���� �̴ϰ��� ���� ����
    public GameObject MinigamePanel;

    [SerializeField] Transform topPivot;        //���� �̴ϰ��� ������ ������ ���� ����
    [SerializeField] Transform bottomPivot;

    [SerializeField] Transform fish;

    private float fishPosition;
    private float fishDestination;

    private float fishTimer;
    [SerializeField] float timerMultiplicator = 3f; //������� ������ �������� ���� ���� 

    private float fishSpeed;
    [SerializeField] float smoothMotion = 1f;       //������� �������� �ε巴��

    [SerializeField] Transform hook;
    [SerializeField] float hookPosition;

    [SerializeField] float hookSize = 0.4f;         //�̴ϰ��� hookArea�� ũ�⸦ ���ϴ� ����
    [SerializeField] float hookPower = 5f;          //hookArea�� ����� ���� hookPullVelocity�� ���õ� ����
    private float hookProgress;
    [SerializeField] float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;   //hookArea�� ����� ���� ����
    [SerializeField] float hookGravityPower = 0.005f;   //hookArea�� �ϰ��� ���� ����
    [SerializeField] float hookProgressDegradationPower = 0.1f; //�̴ϰ��� ������ ������ ��� ���� ����

    [SerializeField] Transform progressBarContainer;

    [SerializeField] float failTimer = 10f;

    //�������� �Ǻ��� ���� ����
    private string zonTag = "";


    private bool isPlayingSound = false;

    void Start()
    {
        //player = GetComponent<Rigidbody>(); // ������ٵ� ������Ʈ �Ҵ�
        anim = GetComponent<Animator>(); // �ִϸ����� ������Ʈ �Ҵ�.
        Controller = GetComponent<PlayerController.ThirdPersonController>();
        AudioM = GetComponent<AudioManager1>();

        fishingPole.SetActive(false); // ���˴� ��Ȱ��ȭ
    }

    void LateUpdate()
    {
        fishTimer -= Time.deltaTime;
        timerToCatch += Time.deltaTime;

        // Ű�� ������ ���� �õ�
        if (!functionTriggered && Input.GetKeyDown(KeyCode.K))
        {
            AudioM.SfxPlay(AudioManager1.Sfx.Cast);
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
            if(!isFishingActive)
                isFishingActive = true;
        }

        else
        {
            isPlayingSound = false;
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

        // ����Ⱑ �̳��� �����µ� �ɸ��� ������ �ð� ����
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
            HitQTE();
        }
        else if (zoneTag == "zone2")
        {
            pullingQTE();
        }
    }

    void HitQTE()
    {
        AudioM.SfxPlay(AudioManager1.Sfx.ReelIn);
        anim.SetBool("ReelIn", true);  //���� ���� �ִϸ��̼��� ����

        qtePanel.SetActive(true);   //QTE��� Ȱ��ȭ
        HitqteSlider.gameObject.SetActive(true);    //QTE�����̴� Ȱ��ȭ

        if (isFishingActive)
        {
            // J Ű�� ������ �� QTE�� �������� ���
            if (Input.GetKeyDown(KeyCode.J))
            {
                anim.SetBool("ReelIn", true);
                currentPresses++; // �Էµ� ��Ÿ Ƚ�� ����
                Debug.Log(currentPresses);
                HitqteSlider.value = (float)currentPresses / requiredPresses; // ��Ÿ Ƚ���� ����Ͽ� �����̴� �� ����
                print("c : " + currentPresses + " r : " + requiredPresses);
                // �ʿ��� ��Ÿ Ƚ���� �����ϸ� ����
                if (currentPresses >= requiredPresses)
                {
                    anim.SetBool("ReelIn", false);
                    anim.SetInteger("Catch", 0);
                    hideHitQTEPanel(); // QTE ���� �� �г� ����
                    ResetHitQTE(); // QTE �ʱ�ȭ
                    Debug.Log("����⸦ ��ҽ��ϴ�!");
                    isFishing = false;
                    anim.SetBool("IsFishing", false);
                    Invoke("DisableFishingPole", 2.0f);
                }
            }

            // �ð��� ���ҿ� ���������� �����ϰ� �� ���� ������ ����
            qteTimer -= Time.deltaTime;
            if (qteTimer <= 0)
            {
                Debug.Log("�ð� �ʰ� - QTE ����!");
                ResetHitQTE(); // �ð� �ʰ� �� QTE �ʱ�ȭ
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
        // QTE �г� ��Ȱ��ȭ �� 1�� �Ŀ� �����̴� ����
        qtePanel.SetActive(false);
        Invoke("hideSlider", 1.0f);
    }

    void hideSlider()
    {
        // �����̴� �����
        HitqteSlider.gameObject.SetActive(false);
    }

    void ResetHitQTE()
    {
        // QTE�� �Էµ� ���� �ʱ�ȭ�ϰ� ���� ������ �غ�
        HitqteSlider.value = 0;
        currentPresses = 0;
        qteTimer = 10.0f;
        timerToCatch = 0;
        isFishingActive = false; // QTE ��Ȱ��ȭ
    }

    void pullingQTE()   //���� �̴ϰ��ӿ� ���õ� �Լ�
    {
        AudioM.SfxPlay(AudioManager1.Sfx.ReelIn);
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
    {   //�̴ϰ��ӿ��� ����Ⱑ ������ ��ġ�� �����̰� �ϱ����� �Լ�
        if (fishTimer < 0f)
        {
            fishTimer = UnityEngine.Random.value * timerMultiplicator;

            fishDestination = UnityEngine.Random.value;
        }
        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }

    void Hook()
    {   //�̴ϰ��ӿ��� ���� �������� ���� �Լ� (���� �ӵ��� �����̴� ��ġ�� ������)
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
    {   //�̴ϰ����� �������� ���,�ϰ���Ű�� ���� �Լ�
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
        AudioM.SfxPlay(AudioManager1.Sfx.Catch);
        AudioM.SfxPlay(AudioManager1.Sfx.Success);
        anim.SetBool("ReelIn", false); ;
        anim.SetInteger("Catch", 0);
        Debug.Log("����⸦ ��Ҵ�!");
        ResetMinigame();
        hideMinigame();
        isFishing = false;
        Invoke("DisableFishingPole", 2.0f);
    }

    void LoseFish()
    {
        AudioM.SfxPlay(AudioManager1.Sfx.Fail);
        anim.SetBool("ReelIn", false); ;
        anim.SetInteger("Catch", 1);
        Debug.Log("����⸦ ���ƴ�!");
        ResetMinigame(); // �ð� �ʰ� �� QTE �ʱ�ȭ
        hideMinigame();
        isFishing = false;
        Invoke("DisableFishingPole", 2.0f);
    }

    void ResetMinigame()
    {
        //�̴ϰ��� �Էµ� �� �ʱ�ȭ
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
        //�̴ϰ��� ��� ����
        MinigamePanel.SetActive(false);
    }

}