using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    Animator animator;
    Animation animation;
    AnimationClip clip;

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

        animator = GetComponent<Animator>();
        //animator.Play()

    }

    private void MoveAction()
    {
        TrackTarget();

    }

    IEnumerator MakeGetAttackedEffect()
    {
        MeshRenderer meshRenderer = transform.GetComponent<MeshRenderer>();
        if (meshRenderer)
        {
            Material saveMaterial = meshRenderer.material;

            meshRenderer.material = Resources.Load<Material>("Materials/Red");
            yield return new WaitForSeconds(0.2f);
            meshRenderer.material = saveMaterial;
            yield return null;
        }
    }

    public void GetAttackedEffect()
    {
        if (Hp > 0)
        {
            StartCoroutine("MakeGetAttackedEffect");
        }
    }

    

    private void Dead()
    {
        //EffectMng.Instance.MakeEffect1(EffectType.EnemyDeadBody, transform.position, 1, 6);
        EffectMng.Instance.MakeEffect1(EffectType.Block1, transform.position, 8, 2f, 15f);
        EnemyUIManager.instance.OnEnemyDead(gameObject);
        EnemyManager.instance.RemoveEnemy(gameObject);
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
            //navMeshAgent.isStopped = true;
        }
    }

    /*
     private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision);
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log(2);
            if ((Time.time - AttackTickCurr) >= AttackTick)
            {
                AttackPlayer(collision.transform);
                AttackTickCurr = Time.time;
            }
        }
    }
     */

    private void AttackPlayer(Transform playerTransform_)
    {
        Debug.Log(playerTransform_);
        PlayerCtrl.Instance.Hp -= 5f;
    }

}
