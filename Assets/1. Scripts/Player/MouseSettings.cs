using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSettings : MonoBehaviour
{
    public static MouseSettings ins;

    private void Awake()
    {
        ins = this;
    }

    public void SetCursorLockMode(CursorLockMode cursorLockMode_ = CursorLockMode.None)
    {
        Cursor.lockState = cursorLockMode_;
    }

    public void MouseVisible()
    {
        Cursor.visible = true;
    }

    public void MouseInvisible()
    {
        Cursor.visible = false;
    }

    public void MouseSetVisible(bool boolen1 = false)
    {
        Cursor.visible = boolen1;
    }

    public void MouseLock(bool boolen1)
    {
        if (boolen1)
        {
            SetCursorLockMode(CursorLockMode.Locked);
            MouseSetVisible(false);
        }
        else
        {
            SetCursorLockMode(CursorLockMode.None);
            MouseSetVisible(true);
        }
    }
    
}
