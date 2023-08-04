using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager instance;
    [SerializeField] private float firstEnemySpawnTime = 45.0f;

    private void Awake()
    {
        instance = this;
    }

    IEnumerator FirstSpawn()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject Enemy_ = SpawnEnemyReturn();
            //EnemyUIManager.instance.OnEnemySpawned(Enemy_);
            yield return new WaitForSeconds(firstEnemySpawnTime);
        }
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            EnemyUIManager.instance.OnEnemySpawned(transform.GetChild(i).gameObject);
        }

        StartCoroutine(FirstSpawn());

    }

    public void SpawnEnemy()
    {
        GameObject NewEnemy = Instantiate(Resources.Load<GameObject>("Prefab/Enemy/Enemy"), transform);
        NewEnemy.name = "New Enemy";
        EnemyUIManager.instance.OnEnemySpawned(NewEnemy);
    }
    public GameObject SpawnEnemyReturn()
    {
        GameObject NewEnemy = Instantiate(Resources.Load<GameObject>("Prefab/Enemy/Enemy"), transform);
        NewEnemy.name = "New Enemy";
        EnemyUIManager.instance.OnEnemySpawned(NewEnemy);
        return NewEnemy;
    }

}
