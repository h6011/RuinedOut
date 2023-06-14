using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    public Transform Target;
    public float MaxHp = 20;
    public float Hp = 20;

    Rigidbody rigidbody;
    NavMeshAgent navMeshAgent;
    BoxCollider collider;

    private float AttackTick = 1.0f;
    private float AttackTickCurr;

    [SerializeField] private float AllowedDistance = 3.0f;

    private void TrackTarget()
    {
        navMeshAgent.SetDestination(Target.position);
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = AllowedDistance;
    }

    private void MoveAction()
    {
        TrackTarget();

    }
    private void Update()
    {
        MoveAction();
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
