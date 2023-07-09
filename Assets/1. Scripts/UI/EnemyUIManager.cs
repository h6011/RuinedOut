using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUIManager : MonoBehaviour
{
    public static EnemyUIManager instance;

    private Camera m_cam;

    [SerializeField] private List<GameObject> EnemyUIList = new List<GameObject>();
    [SerializeField] private List<GameObject> EnemyObjList = new List<GameObject>();
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        m_cam = Camera.main;
        //Resources.Load<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < EnemyUIList.Count; i++)
        {
            if (EnemyUIList[i] & EnemyObjList[i])
            {
                GameObject EnemyUI_ = EnemyUIList[i];
                EnemyCtrl enemyCtrl_ = EnemyObjList[i].GetComponent<EnemyCtrl>();
                Transform frontBar_ = EnemyUI_.transform.Find("FrontBar");
                frontBar_.localScale = new Vector2(Mathf.Clamp01(enemyCtrl_.Hp / enemyCtrl_.MaxHp), 1);
                EnemyUI_.transform.LookAt(m_cam.transform.position);
            }
        }
    }

    public void OnEnemySpawned(GameObject Enemy_)
    {
        Transform FindCanvas = Enemy_.transform.Find("Canvas");
        // Instantiate(Resources.Load<GameObject>("Prefab/UI/EnemyUI"), FindCanvas);
        GameObject EnemyUI = PoolingMng.Instance.CreateObj(PoolingObj.EnemyUI, FindCanvas);
        EnemyUI.name = "EnemyUI";
        EnemyUIList.Add(EnemyUI);
        EnemyObjList.Add(Enemy_);
    }

    public void OnEnemyDead(GameObject Enemy_)
    {
        for (int i = 0; i < EnemyObjList.Count; i++)
        {
            if (EnemyObjList[i] == Enemy_)
            {
                Debug.Log(EnemyUIList[i]);
                PoolingMng.Instance.RemoveObj(EnemyUIList[i], PoolingObj.EnemyUI);
                EnemyObjList.RemoveAt(i);
                EnemyUIList.RemoveAt(i);
            }
        }
    }
}
