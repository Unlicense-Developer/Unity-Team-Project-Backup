using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform target;
    float speed = 10.0f;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = gameObject.transform.forward * verticalInput + gameObject.transform.right * horizontalInput;

        rigid.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }
}
