using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject arrowGauge;

    AudioSource attackSound;

    float time = 0.0f;
    float fireTime = 2.0f;
    float fireForce = 70.0f;
    float throwUpwardForce;

    bool readyToFire = true;

    // Start is called before the first frame update
    void Start()
    {
        attackSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateArrowGauge();

        if ( !readyToFire)
        {
            time += Time.deltaTime;

            if (time >= fireTime)
            {
                readyToFire = true;
                arrowGauge.SetActive(false);
                time = 0.0f;
            }
        }

        if( Input.GetMouseButtonDown(0) && readyToFire)
        {
            Fire();
        }
    }

    void Fire()
    {
        if ( !DefenceGameManager.Instance.IsPlaying() )
            return;

        readyToFire = false;
        arrowGauge.SetActive(true);
        attackSound.Play();

        Quaternion arrowQuat = Quaternion.Euler(new Vector3(0, Camera.main.transform.rotation.eulerAngles.y - 90.0f, -Camera.main.transform.rotation.eulerAngles.x - 90.0f));

        //인스턴스화
        GameObject arrowTemp = Instantiate(arrow, attackPoint.position, arrowQuat);
        Rigidbody arrowRigid = arrowTemp.GetComponent<Rigidbody>();

        //Raycast로 화살의 방향벡터를 계산
        Vector3 forceDirection = Camera.main.transform.forward;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }

        //화살에 가해질 힘
        Vector3 forceToAdd = forceDirection * fireForce + transform.up * throwUpwardForce;

        //화살의 Rigidbody에 물리 연산
        arrowRigid.AddForce(forceToAdd, ForceMode.Impulse);
    }

    void UpdateArrowGauge()
    {
        arrowGauge.transform.Find("Fill").GetComponent<Image>().fillAmount = time / fireTime;
    }
}
