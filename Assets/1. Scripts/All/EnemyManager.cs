using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            EnemyUIManager.instance.OnEnemySpawned(transform.GetChild(i).gameObject);
        }
    }

    public void SpawnEnemy()
    {
        GameObject NewEnemy = Instantiate(Resources.Load<GameObject>("Prefab/Enemy/Enemy"), transform);
        NewEnemy.name = "New Enemy";
        EnemyUIManager.instance.OnEnemySpawned(NewEnemy);
    }

}
