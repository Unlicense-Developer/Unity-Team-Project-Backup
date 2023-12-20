using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookAtTarget : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsTarget();
    }

    void RotateTowardsTarget()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if( Physics.Raycast(ray, out hit))
        {
            RotateTowardsMouse(hit.point);
        }
    }

    void RotateTowardsMouse(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        transform.forward = direction;

    }
}
