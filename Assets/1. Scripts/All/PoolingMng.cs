using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Enemy UI 랑 Item Frame 풀링 처리 해서 할려 했으나 Transform 이니 RectTransform 이니 뭔 문제가 있음;;
 * 
 * 
 * 
 */

public enum PoolingObj
{
    EnemyUI,
    ItemFrame,

}

public class PoolingMng : MonoBehaviour
{
    public static PoolingMng Instance;

    private void Awake()
    {
        Instance = this;
    }

    private GameObject createObj(PoolingObj poolingObj_, Transform Parent)
    {
        GameObject[] objs_ = Resources.LoadAll<GameObject>("Prefab");
        for (int i = 0; i < objs_.Length; i++)
        {
            if (objs_[i].name == poolingObj_.ToString())
            {
                GameObject obj_ =  Instantiate(objs_[i], Parent);
                return obj_;
            }
        }
        return null;
    }

    public GameObject CreateObj(PoolingObj poolingObj_, Transform Parent)
    {
        Transform isThere = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == poolingObj_.ToString())
            {
                isThere = transform.GetChild(i);
                break;
            }
        }

        if (isThere)
        {
            Debug.Log(isThere);
            isThere.gameObject.SetActive(true);
            isThere.parent = Parent;
        }
        else
        {
            isThere = createObj(poolingObj_, Parent).transform;
        }

        return isThere.gameObject;
    }

    public void RemoveObj(GameObject gameObject_)
    {
        gameObject_.SetActive(false);
        gameObject_.transform.parent = transform;
    }


}
