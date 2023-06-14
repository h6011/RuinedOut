using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSettings : MonoBehaviour
{
    public static CameraSettings instance;
    private void Awake()
    {
        instance = this;
        MainCamera = Camera.main;
    }
    public Camera MainCamera;
}
