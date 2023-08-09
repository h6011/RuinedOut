using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    public Transform Target;
    public float MaxHp = 20;
    public float Hp = 20;

    public bool isDead = false;

    NavMeshAgent navMeshAgent;
    CapsuleCollider capsuleCollider;

    private float AttackTick = 1.0f;
    private float AttackTickCurr;

    [SerializeField] private float AllowedDistance = 3.0f;

    private void TrackTarget()
    {
        navMeshAgent.SetDestination(Target.position);
    }

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = AllowedDistance;
        Target = PlayerCtrl.Instance.transform;
    }

    private void MoveAction()
    {
        TrackTarget();

    }

    private void Dead()
    {
        //EffectMng.Instance.MakeEffect1(EffectType.EnemyDeadBody, transform.position, 1, 6);
        EffectMng.Instance.MakeEffect1(EffectType.Block1, transform.position, 8, 2f, 15f);
        EnemyUIManager.instance.OnEnemyDead(gameObject);
        Destroy(gameObject);
    }

    private void CheckHp()
    {
        if (Hp <= 0)
        {
            if (isDead == false)
            {
                isDead = true;
                Dead();
            }
        }
    }

    private void Update()
    {
        CheckHp();
        if (isDead == false)
        {
            MoveAction();
        }
        else
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.isStopped = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(1);
        if (collision.transform.CompareTag("Player"))
        {
            if ((Time.time - AttackTickCurr) >= AttackTick)
            {
                AttackPlayer(collision.transform);
                AttackTickCurr = Time.time;
            }
        }
    }

    private void AttackPlayer(Transform playerTransform_)
    {
        Debug.Log(playerTransform_);
        PlayerCtrl.Instance.Hp -= 5f;
    }

}
