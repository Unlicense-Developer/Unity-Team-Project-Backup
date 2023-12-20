using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interUI;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerPos();
    }

    void CheckPlayerPos()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= 2.0f)
            interUI.SetActive(true);
        else
            interUI.SetActive(false);

    }

}
