using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActive : MonoBehaviour
{
    public GameObject sitBoss;
    public GameObject stendBoss;
    public GameObject ActiveEffect;

    public static BossActive instance;
    public int ActivatedDevices = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        sitBoss.SetActive(true);
        stendBoss.SetActive(false);
        ActiveEffect.SetActive(false);
    }

    public void DeviceActivated()
    {
        ActivatedDevices++;
        Checked();
    }

    private void Checked()
    {
        if (ActivatedDevices >= 2)
        {
            sitBoss.SetActive(false);
            EventCameraController.Instacne.OtherEvnetOn();
            stendBoss.SetActive(true);
            StartCoroutine(SummonEffect());
        }
        else
        {
            Debug.Log("한개 남았습니다");
        }
    }

    IEnumerator SummonEffect()
    {
        ActiveEffect.SetActive(true);

        yield return new WaitForSeconds(10f);
        WorldSoundManager.Instance.PlaySFX("Summon");

        ActiveEffect.SetActive(false);
    }

    void Update()
    {
        
    }
}
