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
            EnemyCtrl enemyCtrl_ = EnemyObjList[i].GetComponent<EnemyCtrl>();
            Transform frontBar_ = EnemyUIList[i].transform.Find("FrontBar");
            frontBar_.localScale = new Vector2( Mathf.Clamp01(enemyCtrl_.Hp / enemyCtrl_.MaxHp) , 1);
            //EnemyUIList[i].transform.position = m_cam.WorldToScreenPoint(EnemyObjList[i].transform.position + new Vector3(0, 1.5f, 0));
            //EnemyUIList[i].transform.position = EnemyObjList[i].transform.position + new Vector3(0, 1.5f, 0);
        }
    }

    public void OnEnemySpawned(GameObject Enemy_)
    {


        GameObject EnemyUI = Instantiate(Resources.Load<GameObject>("Prefab/UI/EnemyUI"), transform);
        EnemyUI.name = "EnemyUI";
        EnemyUIList.Add(EnemyUI);
        EnemyObjList.Add(Enemy_);


    }
}
