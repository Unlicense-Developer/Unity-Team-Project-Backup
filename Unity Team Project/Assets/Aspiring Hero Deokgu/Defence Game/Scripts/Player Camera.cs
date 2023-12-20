using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    float sensitivity = 100.0f;

    float xRot;
    float yRot;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!DefenceGameManager.instance.IsPlaying())
            return;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        xRot -= mouseY;
        yRot += mouseX;

        // x, y Rotation °ª Á¦ÇÑ
        xRot = Mathf.Clamp(xRot, 5.0f, 75.0f);
        yRot = Mathf.Clamp(yRot, 45.0f, 135.0f);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
    }
}
