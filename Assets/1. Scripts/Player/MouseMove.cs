using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseMove : MonoBehaviour
{
    public float mouseSensitivity = 1f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        MouseSettings.ins.MouseLock(true);
    }

    void Move1()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; // Time.deltaTime
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        CameraShake.instance.SaveLocalRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
    
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Move1();
        }
    }
    
}
