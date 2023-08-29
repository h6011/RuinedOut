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

    private float enemyCanFindDistance = 10.0f; // 좀비 시야
    private float enemyAttackTick = 2.0f; // 몇초 간격으로 때리게 할건지
    private float enemyAttackCurrTick = 0.0f; // 건드리지 마세요!
    private float enemyAttackDamage = 4.0f; // 
    private float enemyAttackDistance = 3f; // 공격 유효 사거리
    [SerializeField] private float AllowedDistance = 2.5f;

    private void TrackTarget()
    {
        if (Target)
        {
            navMeshAgent.SetDestination(Target.position);
        }
    }

    public void SetTarget(Transform targetTransform)
    {
        Target = targetTransform;
    }

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = AllowedDistance;
        navMeshAgent.enabled = true;

        animator = GetComponent<Animator>();

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

    
    public void DeadFunctionForAniEvent()
    {
        EffectMng.Instance.MakeEffect1(EffectType.Block1, transform.position, 8, 2f, 15f);
        EnemyUIManager.instance.OnEnemyDead(gameObject);
        EnemyManager.instance.RemoveEnemy(gameObject);
    }

    private void Dead()
    {
        gameObject.layer = LayerMask.NameToLayer("EnemyDead");
        gameObject.tag = "EnemyDead";
        navMeshAgent.enabled = false;
        animator.SetInteger("Ani", 3);
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

    private void AttackAction()
    {
        if (Target)
        {
            float Distance_ = Vector3.Distance(transform.position, Target.position);
            if (Distance_ <= enemyAttackDistance)
            {
                if ((Time.time - enemyAttackCurrTick) >= enemyAttackTick)
                {
                    enemyAttackCurrTick = Time.time;
                    animator.SetTrigger("Attack");
                }
            }
        }
    }

    private void CheckCanFind()
    {
        if (Target == null)
        {
            float Distance_ = Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position);
            if (Distance_ <= enemyCanFindDistance)
            {
                SetTarget(PlayerCtrl.Instance.transform);
            }
        }
        else
        {
            float Distance_ = Vector3.Distance(transform.position, Target.position);
            if (Distance_ > enemyCanFindDistance)
            {
                SetTarget(null);
            }
            else
            {
                Vector3 curRot = transform.rotation.eulerAngles;
                transform.LookAt(Target.position);
                float yAngle = transform.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Euler(curRot.x, yAngle, curRot.z); 
            }
        }
    }

    private void AnimationAction()
    {
        if (isDead)
        {
            animator.SetInteger("Ani", 3);
        }
        else
        {
            if (navMeshAgent.velocity.magnitude > 0)
            {
                if (Target)
                {
                    animator.SetInteger("Ani", 2);
                }
                else
                {
                    animator.SetInteger("Ani", 1);
                }
            }
            else
            {
                animator.SetInteger("Ani", 0);
            }
        }
    }

    private void Update()
    {
        CheckCanFind();
        CheckHp();
        AttackAction();
        if (isDead == false)
        {
            MoveAction();
        }
        AnimationAction();
    }

  

     //private void OnCollisionEnter(Collision collision)
     //{
     //   Debug.Log(collision.transform.name);
     //   if (collision.transform.CompareTag("Player"))
     //   {
     //       if ((Time.time - AttackTickCurr) >= AttackTick)
     //       {
     //           AttackPlayer(collision.transform);
     //           AttackTickCurr = Time.time;
     //       }
     //   }
     //}


    private void TryAttackPlayer()
    {
        Debug.Log("TryAttack");
        float Dis = Vector3.Distance(transform.position, Target.position);
        if (Dis <= enemyAttackDistance)
        {
            PlayerCtrl.Instance.Hp -= enemyAttackDamage;
        }
    }

}
