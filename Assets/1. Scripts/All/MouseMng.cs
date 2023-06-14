using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMng : MonoBehaviour
{
    public static MouseMng ins;

    private void Awake()
    {
        ins = this;
    }

    public RaycastHit GetRaycastHit(LayerMask layermask)
    {
        Ray ray = CameraSettings.instance.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,  layermask))
        {
            return hit;
        }
        return hit;
    }

    public Transform GetMouseTarget(LayerMask layermask)
    {
        RaycastHit RayHit1 = GetRaycastHit(layermask);
        return RayHit1.transform;
    }

    public float GetDistanceFromTarget(LayerMask layermask)
    {
        RaycastHit RayHit1 = GetRaycastHit(layermask);
        return RayHit1.distance;
    }






}
