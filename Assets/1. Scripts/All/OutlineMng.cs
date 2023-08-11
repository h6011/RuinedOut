using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineMng : MonoBehaviour
{

    public static OutlineMng Instance;

    [SerializeField] List<Outline> outlineList = new List<Outline>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        int childcount = ItemManager.Instance.transform.childCount;
        for (int i = 0; i < childcount; i++)
        {
            Transform obj_ = ItemManager.Instance.transform.GetChild(i);
            Outline IsSetupedOutline_ = IsSetupedOutline(obj_.gameObject);
            if (IsSetupedOutline_)
            {
                outlineList.Add(IsSetupedOutline_);
            }
            else
            {
                SetupOutline(obj_.gameObject);
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < outlineList.Count; i++)
        {
            Outline obj = outlineList[i];
            if (obj)
            {
                obj.enabled = true;
            }
        }
    }

    public GameObject SetupOutline(GameObject Obj)
    {
        Outline outlineScript = Obj.GetComponent<Outline>();
        if (outlineScript == null)
        {
            Outline NewOutline = Obj.AddComponent<Outline>();
            NewOutline.enabled = false;
            outlineList.Add(NewOutline);
            return NewOutline.gameObject;
        }
        return null;
    }

    public Outline IsSetupedOutline(GameObject Obj)
    {
        Outline outlineScript = Obj.GetComponent<Outline>();
        if (outlineScript)
        {
            return outlineScript;
        }
        return null;
    }


    public void DisableAllOutlines()
    {
        for (int i = 0; i < outlineList.Count; i++)
        {
            Outline obj = outlineList[i];
            if (obj)
            {
                //obj.enabled = false;
                Color color = obj.OutlineColor;
                color.a = 0.0f;
                obj.OutlineColor = color;
            }
        }
    }


    public void EnableAllOutlines()
    {
        for (int i = 0; i < outlineList.Count; i++)
        {
            Outline obj = outlineList[i];
            obj.enabled = true;
        }
    }









}
