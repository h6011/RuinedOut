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

    private void CheckHp()
    {
        if (Hp <= 0)
        {
            isDead = true;
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
            navMeshAgent.isStopped = true;
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
    }

}
