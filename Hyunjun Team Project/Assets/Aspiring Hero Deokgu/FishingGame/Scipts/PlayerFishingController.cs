using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerFishingController : MonoBehaviour
{
    PlayerController.ThirdPersonController Controller;
    Animator anim; // �ִϸ�����

    public TextMeshProUGUI Alarm;   //����Ⱑ �̳��� ���� �ߵ��Ǵ� �ؽ�Ʈ

    private bool canFishA; // ���� ���� ���� �Ǵ��� ���� ����
    private bool canFishB;

    public int FishingResult;
    public float timerToCatch; // ���� �ð�
    public float timeBeforeBite; // ����Ⱑ �̳��� ��������� �ð�
    public bool isFishing; // �÷��̾ ���� �� ���� �Ǵ��� ���� ����

    private bool functionTriggered = false; //���� �ִϸ��̼� ���� �� ������� ���� ����

    public GameObject fishingZone_1; // ���� ����1
    public GameObject fishingZone_2; // ���� ����2

    public GameObject fishingPole; // ���˴� ���� ������Ʈ

    public Text Fish1;
    public Text Fish2;
    public Text Fish3;
    public Text Fish4;
    public Text Fail;

    public bool isFishingActive = true; //���ý��� �� ������� ���� ����

    //��Ÿ�� QTE ���� ����
    public int requiredPresses; // �ʿ��� ��Ÿ Ƚ��
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
    [SerializeField] float hookPullPower = 0.01f;       //hookArea�� ����� ���� ����
    [SerializeField] float hookGravityPower = 0.005f;   //hookArea�� �ϰ��� ���� ����
    [SerializeField] float hookProgressDegradationPower = 0.1f; //�̴ϰ��� ������ ������ ��� ���� ����

    [SerializeField] Transform progressBarContainer;

    [SerializeField] float failTimer = 10f;

    //�������� �Ǻ��� ���� ����
    private string zonTag = "";


    private void Start()
    {
        //player = GetComponent<Rigidbody>(); // ������ٵ� ������Ʈ �Ҵ�
        anim = GetComponent<Animator>(); // �ִϸ����� ������Ʈ �Ҵ�.
        Controller = GetComponent<PlayerController.ThirdPersonController>();

        fishingPole.SetActive(false); // ���˴� ��Ȱ��ȭ

        Fish1.color = new Color(Fish1.color.r, Fish1.color.g, Fish1.color.b, 0f);
        Fish2.color = new Color(Fish2.color.r, Fish2.color.g, Fish2.color.b, 0f);
        Fish3.color = new Color(Fish3.color.r, Fish3.color.g, Fish3.color.b, 0f);
        Fish4.color = new Color(Fish4.color.r, Fish4.color.g, Fish4.color.b, 0f);
        Fail.color = new Color(Fail.color.r, Fail.color.g, Fail.color.b, 0f);
    }

    private void LateUpdate()
    {
        fishTimer -= Time.deltaTime;
        timerToCatch += Time.deltaTime;

        // Ű�� ������ ���� �õ�
        if (!functionTriggered && Input.GetKeyDown(KeyCode.G))
        {
            functionTriggered = true;   //���۽���
            FishCheck();
            functionTriggered = false;  //���� ������� ���� ����
        }

        // �̵� �� ���� �ߴ�
        if (Controller._isMoving == true && isFishing)
        {
            Debug.Log("�������� ���ð� ��ҵǾ����ϴ�.");
            isFishing = false;
            fishingPole.SetActive(false);
            anim.SetBool("IsFishing", false);
        }

        // �������� �ð��� ������ ����Ⱑ �̳��� ����
        if (timerToCatch > timeBeforeBite && isFishing && !Controller._isMoving)
        {
            //Debug.Log("Hit!!!");
            FishingZoneEntered(zonTag);
            if(!isFishingActive)
                isFishingActive = true;
        }
    }

    IEnumerator CastSound()
    {
        yield return new WaitForSeconds(2f);
        WorldSoundManager.Instance.PlaySFX("Cast");
    }

    private void DisableFishingPole()
    {
        fishingPole.SetActive(false);
    }

    private void FishCheck()
    {
        canFishA = fishingZone_1.GetComponent<FishingZone_>().PlayerCanFish(); //���ð� �������� üũ
        canFishB = fishingZone_2.GetComponent<FishingZone_>().PlayerCanFish();

        if (canFishA || canFishB)
        {
            anim.SetBool("IsFishing", true);
            fishingPole.SetActive(true);
            Fish(); // ���� ����
            Debug.Log("���� ���Դϴ�!");
        }
        else
        {
            Debug.Log("�̰������� ������ �� �����ϴ�!");
            fishingPole.SetActive(false);
        }
    }

    private void Fish()
    {
        FishingResult = Random.Range(1,6);
        Debug.Log(FishingResult);
        timerToCatch = 0f;
        timeBeforeBite = 0f;
        System.Random random = new System.Random();

        // ����Ⱑ �̳��� �����µ� �ɸ��� ������ �ð� ����
        timeBeforeBite = random.Next(10) + 5;

        //���õ����� ����
        Debug.Log(timeBeforeBite);
        isFishing = true;
        StartCoroutine(CastSound());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("zone1"))
        {
            zonTag = other.gameObject.tag;
            //FishingZoneEntered("zone1");
        }
        else if (other.CompareTag("zone2"))
        {
            zonTag = other.gameObject.tag;
            //FishingZoneEntered("zone2");
        }
    }

    private void FishingZoneEntered(string zoneTag)
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

    private void HitQTE()
    {
        AnimateText();
        anim.SetBool("ReelIn", true);  //���� ���� �ִϸ��̼��� ����

        qtePanel.SetActive(true);   //QTE��� Ȱ��ȭ
        HitqteSlider.gameObject.SetActive(true);    //QTE�����̴� Ȱ��ȭ

        Controller.enabled = false;

        if (isFishingActive && !Controller._isMoving)
        {
            // ���콺�� ��Ÿ���� �� QTE�� �������� ���
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetBool("ReelIn", true);
                currentPresses++;
                HitqteSlider.value = (float)currentPresses / requiredPresses; // ��Ÿ Ƚ���� ����Ͽ� �����̴� �� ����
                // �ʿ��� ��Ÿ Ƚ���� �����ϸ� ����
                if (currentPresses >= requiredPresses)
                {
                    anim.SetBool("ReelIn", false);
                    anim.SetInteger("Catch", 0);
                    StartCoroutine(SuccessSound());
                    hideHitQTEPanel(); // QTE ���� �� �г� ����
                    ResetHitQTE(); // QTE �ʱ�ȭ
                    Debug.Log("����⸦ ��ҽ��ϴ�!");
                    isFishing = false;
                    Invoke("DisableFishingPole", 2.0f);

                    if (FishingResult == 1 || FishingResult == 3 || FishingResult == 5)
                    {
                        InventoryManager.Instance.AddItem("Bass");
                        ResultText();
                    }
                    else
                    {
                        InventoryManager.Instance.AddItem("Trout");
                        ResultText();
                    }
                }
            }

            // �ð��� ���ҿ� ���������� �����ϰ� �� ���� ������ ����
            qteTimer -= Time.deltaTime;
            if (qteTimer <= 0)
            {
                Debug.Log("�ð� �ʰ� - QTE ����!");
                StartCoroutine(FailSound());
                ResetHitQTE(); // �ð� �ʰ� �� QTE �ʱ�ȭ
                hideHitQTEPanel();
                hideSlider();
                anim.SetBool("ReelIn", false);
                anim.SetInteger("Catch", 1);
                isFishing = false;
                Invoke("DisableFishingPole", 2.0f);
            }
        }
    }

    private void hideHitQTEPanel()
    {
        // QTE �г� ��Ȱ��ȭ �� 1�� �Ŀ� �����̴� ����
        qtePanel.SetActive(false);
        Invoke("hideSlider", 1.0f);
    }

    private void hideSlider()
    {
        // �����̴� �����
        HitqteSlider.gameObject.SetActive(false);
    }

    private void ResetHitQTE()
    {
        // QTE�� �Էµ� ���� �ʱ�ȭ�ϰ� ���� ������ �غ�
        HitqteSlider.value = 0;
        currentPresses = 0;
        qteTimer = 10.0f;
        timerToCatch = 0;
        isFishingActive = false; // QTE ��Ȱ��ȭ
        anim.SetBool("IsFishing", false);
        Controller.enabled = true;
    }

    private void pullingQTE()   //���� �̴ϰ��ӿ� ���õ� �Լ�
    {
        anim.SetBool("ReelIn", true);
        AnimateText();
        Controller.enabled = false;

        if (isFishingActive && !Controller._isMoving)
        {
            MinigamePanel.SetActive(true);
            fishing();
            Hook();
            ProgressCheck();
        }
    }

    private void fishing()
    {   //�̴ϰ��ӿ��� ����Ⱑ ������ ��ġ�� �����̰� �ϱ����� �Լ�
        if (fishTimer < 0f)
        {
            fishTimer = UnityEngine.Random.value * timerMultiplicator;

            fishDestination = UnityEngine.Random.value;
        }
        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }

    private void Hook()
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

    private void ProgressCheck()
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

    private void Catch()
    {
        StartCoroutine(SuccessSound());
        anim.SetBool("ReelIn", false); ;
        anim.SetInteger("Catch", 0);
        Debug.Log("����⸦ ��Ҵ�!");
        ResetMinigame();
        hideMinigame();
        isFishing = false;
        Invoke("DisableFishingPole", 2.0f);

        if (FishingResult == 1 || FishingResult == 2 || FishingResult == 3 || FishingResult == 4)
        {
            InventoryManager.Instance.AddItem("Salmon");
            ResultText();
        }
        else
        {
            InventoryManager.Instance.AddItem("Tuna");
            AchievementManager.Instance.SetAchieveValue("Fishing", 1);
            ResultText();
        }
    }
    private void LoseFish()
    {
        anim.SetBool("ReelIn", false); ;
        anim.SetInteger("Catch", 1);
        StartCoroutine(FailSound());
        Debug.Log("����⸦ ���ƴ�!");
        ResetMinigame(); // �ð� �ʰ� �� QTE �ʱ�ȭ
        hideMinigame();
        isFishing = false;
        Invoke("DisableFishingPole", 2.0f);
    }
    private void ResetMinigame()
    {
        //�̴ϰ��� �Էµ� �� �ʱ�ȭ
        failTimer = 10.0f;
        fishPosition = 0.1f;
        hookPullVelocity = 0f;
        hookProgress = 0;
        hookPosition = 0.1f;
        timerToCatch = 0;
        anim.SetBool("IsFishing", false);
        isFishingActive = false;
        Controller.enabled = true;
    }

    private void hideMinigame()
    {
        //�̴ϰ��� ��� ����
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

    public void RandomFishing()
    {
        requiredPresses = Random.Range(15, 26);
    }

    public void PlayReelIn()
    {
        WorldSoundManager.Instance.PlaySFX("ReelIn");
        Controller.enabled = false;
    }

    private void AnimateText()
    {
        Alarm.gameObject.SetActive(true);
        Vector3 initialPosition = Alarm.transform.position;

        Alarm.transform.DOMove(new Vector3(initialPosition.x + 10f, initialPosition.y + 10f, initialPosition.z), 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                Alarm.DOFade(0f, 0.5f).SetDelay(0.1f).OnComplete(() =>
                {
                    Alarm.gameObject.SetActive(false);
                });
            });
    }

    private void ResultText()
    {
        if (canFishA && FishingResult == 1 || FishingResult == 3 || FishingResult == 5)
        {
            Fish1.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // 3�� �Ŀ� �ؽ�Ʈ�� ������ ������� �ִϸ��̼�
                DOVirtual.DelayedCall(3f, () =>
                {
                    Fish1.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                    {
                    });
                });
            });
        }

        else if (canFishA && FishingResult == 2 || FishingResult == 4)
        {
            Fish2.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                DOVirtual.DelayedCall(3f, () =>
                {
                    Fish2.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                    {
                    });
                });
            });
        }

        else if (canFishB && FishingResult == 1 || FishingResult == 2 || FishingResult == 3 || FishingResult == 4)
        {
            Fish3.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                DOVirtual.DelayedCall(3f, () =>
                {
                    Fish3.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                    {
                    });
                });
            });
        }

        else if (canFishB && FishingResult == 5)
        {
            Fish4.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // 3�� �Ŀ� �ؽ�Ʈ�� ������ ������� �ִϸ��̼�
                DOVirtual.DelayedCall(3f, () =>
                {
                    Fish4.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                    {
                    });
                });
            });
        }

        else
        {
            Fail.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                // 3�� �Ŀ� �ؽ�Ʈ�� ������ ������� �ִϸ��̼�
                DOVirtual.DelayedCall(3f, () =>
                {
                    Fail.DOFade(0f, 1f).SetEase(Ease.OutQuad).OnComplete(() =>
                    {
                    });
                });
            });
        }
    }
}