using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * 
 * 
 * 
 */

public enum PoolingObj
{
    EnemyUI,
    ItemFrame,

    EnemyDeadBody,
    PlayerDeadBody,
    Block1,
    
    Zombie1,

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
                GameObject obj_ = Instantiate(objs_[i], Parent);
                return obj_;
            }
        }
        return null;
    }

    private GameObject createObj(string poolingObjname, Transform Parent)
    {
        GameObject[] objs_ = Resources.LoadAll<GameObject>("Prefab");
        for (int i = 0; i < objs_.Length; i++)
        {
            if (objs_[i].name == poolingObjname)
            {
                GameObject obj_ = Instantiate(objs_[i], Parent);
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
            isThere.gameObject.SetActive(true);
            isThere.SetParent(Parent);
        }
        else
        {
            isThere = createObj(poolingObj_, Parent).transform;
        }

        return isThere.gameObject;
    }

    public GameObject CreateObj(string poolingObjname, Transform Parent)
    {
        Transform isThere = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == poolingObjname)
            {
                isThere = transform.GetChild(i);
                break;
            }
        }

        if (isThere)
        {
            EnemyCtrl enemyCtrl = isThere.transform.GetComponent<EnemyCtrl>();
            if (enemyCtrl)
            {
                enemyCtrl.DoWhenAlive();
            }
            isThere.gameObject.SetActive(true);
            isThere.SetParent(Parent);
        }
        else
        {
            isThere = createObj(poolingObjname, Parent).transform;
        }

        return isThere.gameObject;
    }

    public void RemoveObj(GameObject gameObject_, PoolingObj poolingObj)
    {
        gameObject_.SetActive(false);
        gameObject_.name = poolingObj.ToString();
        gameObject_.transform.SetParent(transform);
    }

    public void RemoveObj(GameObject gameObject_, string poolingObjname)
    {
        gameObject_.SetActive(false);
        gameObject_.name = poolingObjname;
        gameObject_.transform.SetParent(transform);
    }



}
