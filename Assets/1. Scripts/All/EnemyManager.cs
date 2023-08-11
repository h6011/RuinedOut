using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager instance;
    [SerializeField] private float firstEnemySpawnTime = 45.0f;
    [SerializeField, Range(1, 200)] private float firstEnemySpawnAmount = 99.0f;

    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();

    [SerializeField] private Dictionary<GameObject, float> enemyAttackTickCurrDic = new Dictionary<GameObject, float>();
    private float enemyAttackTick = 2.0f;
    private float enemyAttackDamage = 4.0f;
    private float enemyAttackDistance = 2f;

    private void Awake()
    {
        instance = this;
    }

    IEnumerator FirstSpawn()
    {
        for (int i = 0; i < firstEnemySpawnAmount; i++)
        {
            GameObject Enemy_ = SpawnEnemyReturn();
            //EnemyUIManager.instance.OnEnemySpawned(Enemy_);
            yield return new WaitForSeconds(firstEnemySpawnTime);
        }
    }

    private void AttackPlayer(float damage)
    {
        PlayerCtrl.Instance.Hp -= damage;
    }

    private void EnemyAttackAction()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            GameObject enemyObj = enemyList[i];
            float outvalue;
            float Distance_ = (enemyObj.transform.position - PlayerCtrl.Instance.PlayerObject.transform.position).magnitude;

            if (Distance_ <= enemyAttackDistance)
            {
                bool TryGetValue_ = enemyAttackTickCurrDic.TryGetValue(enemyObj, out outvalue);
                if (TryGetValue_)
                {
                    if ((Time.time - outvalue) >= enemyAttackTick)
                    {
                        AttackPlayer(enemyAttackDamage);
                        enemyAttackTickCurrDic.Remove(enemyObj);
                        enemyAttackTickCurrDic.Add(enemyObj, Time.time);
                    }
                }
                else
                {
                    AttackPlayer(enemyAttackDamage);
                    enemyAttackTickCurrDic.Add(enemyObj, Time.time);
                }
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            enemyList.Add(transform.gameObject);
            EnemyUIManager.instance.OnEnemySpawned(transform.GetChild(i).gameObject);
        }

        StartCoroutine(FirstSpawn());

    }
    private void Update()
    {
        EnemyAttackAction();
    }
    
    public void SpawnEnemy()
    {
        GameObject NewEnemy = Instantiate(Resources.Load<GameObject>("Prefab/Enemy/Enemy"), transform);
        NewEnemy.name = "New Enemy";
        enemyList.Add(NewEnemy);
        EnemyUIManager.instance.OnEnemySpawned(NewEnemy);
    }
    public GameObject SpawnEnemyReturn()
    {
        GameObject NewEnemy = Instantiate(Resources.Load<GameObject>("Prefab/Enemy/Enemy"), transform);
        NewEnemy.name = "New Enemy";
        enemyList.Add(NewEnemy);
        EnemyUIManager.instance.OnEnemySpawned(NewEnemy);
        return NewEnemy;
    }

    public void RemoveEnemy(GameObject enemyObj)
    {
        enemyList.Remove(enemyObj);
        Destroy(enemyObj);
    }

}
