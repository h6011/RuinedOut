using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private Camera mainCamera;
    //public Vector3 SaveLocalPos;
    //public Quaternion SaveLocalRotation;

    [SerializeField] Vector3 test;
    [SerializeField] Quaternion test2;

    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
        //SaveLocalPos = mainCamera.transform.localPosition;
        //SaveLocalRotation = mainCamera.transform.localRotation;
    }


    //public void CameraShake1(float x, float y)
    //{
    //    mainCamera.transform.localRotation = test2;
    //}

    //public void CameraShake2(float x, float y)
    //{
    //    mainCamera.transform.localPosition = test;
    //}

    public IEnumerator CameraRightUpShake1()
    {
        float x = -45f;
        float y = 45f;

        mainCamera.transform.localEulerAngles += new Vector3(x, y);

        float t1 = Time.time;
        float t2 = 1f;

        while ( (Time.time - t1) <= t2)
        {
            mainCamera.transform.localEulerAngles = Vector3.Lerp(mainCamera.transform.localEulerAngles, new Vector3(0, 0), (Time.time - t1));
            yield return null;
        }


    }

    //private void Update()
    //{
    //    mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, SaveLocalPos, 0.2f * Time.deltaTime);
    //    mainCamera.transform.localRotation = Quaternion.Lerp(mainCamera.transform.localRotation, SaveLocalRotation, 0.2f * Time.deltaTime);

    //}






}
