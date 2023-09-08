using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager instance;

    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();

    #region RandomSpawn Vars
    [Header("RandomSpawn")]
    [SerializeField] private GameObject ZombieSpawnRegion;

    [SerializeField] private int MaxRandomEnemySpawn = 20;
    [SerializeField] private float RandomEnemySpawnTime = 1.0f;
    [SerializeField] private LayerMask toCheckCapsule;

    #endregion

    //[SerializeField] private Dictionary<GameObject, float> enemyAttackTickCurrDic = new Dictionary<GameObject, float>();
    //private float enemyAttackTick = 2.0f;
    //private float enemyAttackDamage = 4.0f;
    //private float enemyAttackDistance = 2f;

    private void Awake()
    {
        instance = this;
        
    }

    IEnumerator EnemyRandomSpawnCo()
    {
        while (true)
        {
            int Count = enemyList.Count;

            if (Count < MaxRandomEnemySpawn)
            {
                Vector3 Pos = ZombieSpawnRegion.transform.position + new Vector3(
                    Random.Range(-ZombieSpawnRegion.transform.localScale.x * 0.5f, ZombieSpawnRegion.transform.localScale.x * 0.5f), 
                    0,
                    Random.Range(-ZombieSpawnRegion.transform.localScale.z * 0.5f, ZombieSpawnRegion.transform.localScale.z * 0.5f)
                    );
                bool checkSphere = Physics.CheckSphere(Pos, 2, toCheckCapsule);
                if (checkSphere)
                {
                    yield return null;
                }
                else
                {
                    GameObject Enemy_ = SpawnEnemy("Zombie1", true);
                    Enemy_.transform.position = Pos;
                    yield return new WaitForSeconds(RandomEnemySpawnTime);
                }
            }
            yield return null;

        }
    }

    //IEnumerator FirstSpawn()
    //{
    //    for (int i = 0; i < firstEnemySpawnAmount; i++)
    //    {
    //        GameObject Enemy_ = SpawnEnemy("Zombie1", true);
    //        Enemy_.transform.position = new Vector3();
    //        yield return new WaitForSeconds(firstEnemySpawnTime);
    //    }
    //}

    private void AttackPlayer(float damage)
    {

        PlayerCtrl.Instance.Hp -= damage;
    }

    //private void EnemyAttackAction()
    //{
    //    for (int i = 0; i < enemyList.Count; i++)
    //    {
    //        GameObject enemyObj = enemyList[i];
    //        float outvalue;
    //        float Distance_ = (enemyObj.transform.position - PlayerCtrl.Instance.PlayerObject.transform.position).magnitude;

    //        if (Distance_ <= enemyAttackDistance)
    //        {
    //            bool TryGetValue_ = enemyAttackTickCurrDic.TryGetValue(enemyObj, out outvalue);
    //            if (TryGetValue_)
    //            {
    //                if ((Time.time - outvalue) >= enemyAttackTick)
    //                {
    //                    AttackPlayer(enemyAttackDamage);
    //                    enemyAttackTickCurrDic.Remove(enemyObj);
    //                    enemyAttackTickCurrDic.Add(enemyObj, Time.time);
    //                }
    //            }
    //            else
    //            {
    //                AttackPlayer(enemyAttackDamage);
    //                enemyAttackTickCurrDic.Add(enemyObj, Time.time);
    //            }
    //        }
    //    }
    //}

    private void Start()
    {
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    enemyList.Add(transform.gameObject);
        //    EnemyUIManager.instance.OnEnemySpawned(transform.GetChild(i).gameObject);
        //}

        StartCoroutine(EnemyRandomSpawnCo());

    }
    //private void Update()
    //{
    //    EnemyAttackAction();
    //}

    public GameObject SpawnEnemy(string Name, bool HaveToReturn = false)
    {
        GameObject NewEnemy = PoolingMng.Instance.CreateObj(Name, transform);
        NewEnemy.name = "New Enemy";
        enemyList.Add(NewEnemy);
        EnemyUIManager.instance.OnEnemySpawned(NewEnemy);
        if (HaveToReturn)
        {
            return NewEnemy;
        }
        else
        {
            return null;
        }
    }

    public void RemoveEnemy(GameObject enemyObj)
    {
        enemyList.Remove(enemyObj);
        Destroy(enemyObj);
    }

}
