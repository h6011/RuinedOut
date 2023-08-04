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
    }

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
        GameObject EnemyUI = PoolingMng.Instance.CreateObj(PoolingObj.EnemyUI, FindCanvas);
        EnemyUI.transform.localPosition = new Vector3(0, 120.0f, 0f);
        EnemyUI.name = "EnemyUI";
        EnemyUIList.Add(EnemyUI);
        EnemyObjList.Add(Enemy_);
    }

    public void OnEnemyDead(GameObject Enemy_)
    {
        int idx = EnemyObjList.IndexOf(Enemy_);
        PoolingMng.Instance.RemoveObj(EnemyUIList[idx], PoolingObj.EnemyUI);
        EnemyObjList.RemoveAt(idx);
        EnemyUIList.RemoveAt(idx);
    }
}
