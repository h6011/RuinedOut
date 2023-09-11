using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    [Tooltip("Enemy's Target")] public Transform Target;

    [Header("Enemy's Stat")]
    [Tooltip("MaxHp Of Enemy")] public float MaxHp = 20;
    [Tooltip("Hp Of Enemy")] public float Hp = 20;
    [Tooltip("WalkSpeed Of Enemy")] [SerializeField] private float WalkSpeed = 4.0f; // �ȴ� �ӵ�

    [HideInInspector] public bool isDead = false;
    [HideInInspector] public bool isAttacking = false;

    NavMeshAgent navMeshAgent;
    CapsuleCollider capsuleCollider;
    Animator animator;


    private float enemyCanFindDistance = 25.0f; // ���� �þ�
    private float enemyAttackTick = 2.0f; // ���� �������� ������ �Ұ���
    private float enemyAttackCurrTick = 0.0f; // �ǵ帮�� ������!
    private float enemyAttackDamage = 4.0f; // 
    private float enemyAttackDistance = 3f; // ���� ��ȿ ��Ÿ�
    [SerializeField] private float AllowedDistance = 2.5f;

    private void TrackTarget()
    {
        if (Target)
        {
            if (isAttacking)
            {
                navMeshAgent.ResetPath();
            }
            else
            {
                navMeshAgent.SetDestination(Target.position);
            }
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
        navMeshAgent.speed = WalkSpeed;
        navMeshAgent.enabled = true;
        Hp = MaxHp;

        animator = GetComponent<Animator>();
    }

    public void DoWhenAlive()
    {
        Hp = MaxHp;
        isDead = false;
        isAttacking = false;
        navMeshAgent.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        gameObject.tag = "Enemy";
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
        EffectMng.Instance.MakeEffect1(EffectType.Block1, transform.position, transform.rotation, 8, 2f, 15f);
        EnemyUIManager.instance.OnEnemyDead(gameObject);
        EnemyManager.instance.RemoveEnemy(gameObject);
    }

    private void Dead()
    {
        gameObject.layer = LayerMask.NameToLayer("EnemyDead");
        gameObject.tag = "EnemyDead";
        navMeshAgent.enabled = false;

        EffectMng.Instance.MakeEffect1(EffectType.Zombie1Dead, transform.position, transform.rotation, 1, 4f, 0f);
        EnemyUIManager.instance.OnEnemyDead(gameObject);
        EnemyManager.instance.RemoveEnemy(gameObject);


        //animator.SetInteger("Ani", 3);
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
                    isAttacking = true;
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


    private void TryAttackPlayer()
    {
        float Dis = Vector3.Distance(transform.position, Target.position);
        if (Dis <= enemyAttackDistance)
        {
            PlayerCtrl.Instance.Hp -= enemyAttackDamage;
            PlayerCtrl.Instance.GetAttackedEffect();
        }
    }

    private void AttackEnd()
    {
        isAttacking = false;
    }

}
