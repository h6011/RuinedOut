using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager instance;
    [SerializeField] private float firstEnemySpawnTime = 45.0f;
    [SerializeField, Range(1, 200)] private float firstEnemySpawnAmount = 99.0f;

    [SerializeField] private List<GameObject> enemyList = new List<GameObject>();

    private float enemyAttackTick = 1.0f;
    [SerializeField] private Dictionary<GameObject, float> enemyAttackTickCurrDic = new Dictionary<GameObject, float>();

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
            bool IsContainKey = enemyAttackTickCurrDic.ContainsKey(enemyObj);
            if (IsContainKey)
            {
                for (int i2 = 0; i2 < length; i2++)
                {

                }
                if ((Time.time - IsContainKey) >= enemyAttackTick)
                {
                    AttackPlayer(collision.transform);
                    AttackTickCurr = Time.time;
                }
            }
            else
            {

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
    private void FixedUpdate()
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
