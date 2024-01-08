using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
    float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rotateSpeed = Random.Range(0.0f, 4.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
