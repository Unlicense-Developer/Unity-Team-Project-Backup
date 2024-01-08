using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    //���� �̴ϰ��� ���� ����
    PlayerController.ThirdPersonController Controller;
    PlayerFishingController FC;
    Animator anim;
    public GameObject MinigamePanel;

    [SerializeField] Transform topPivot;        //���� �̴ϰ��� ������ ������ ���� ����
    [SerializeField] Transform bottomPivot;

    [SerializeField] Transform fish;

    private float fishPosition;
    private float fishDestination;

    public float fishTimer;
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

    public bool fishingfalse = false;

    [SerializeField] float failTimer = 10f;

    private void Start()
    {
        Controller = GetComponent<PlayerController.ThirdPersonController>();
        FC = GetComponent<PlayerFishingController>();
    }

    public void pullingQTE()   //���� �̴ϰ��ӿ� ���õ� �Լ�
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
        anim.SetBool("ReelIn", false); ;
        anim.SetInteger("Catch", 0);
        Debug.Log("����⸦ ��Ҵ�!");
        ResetMinigame();
        hideMinigame();
        FC.isFishing = false;
        Invoke("DisableFishingPole", 2.0f);
    }

    void LoseFish()
    {
        anim.SetBool("ReelIn", false); ;
        anim.SetInteger("Catch", 1);
        Debug.Log("����⸦ ���ƴ�!");
        ResetMinigame(); // �ð� �ʰ� �� QTE �ʱ�ȭ
        hideMinigame();
        FC.isFishing = false;
        Invoke("DisableFishingPole", 2.0f);
    }

    void ResetMinigame()
    {
        //�̴ϰ��� �Էµ� �� �ʱ�ȭ
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
        //�̴ϰ��� ��� ����
        MinigamePanel.SetActive(false);
    }
}
