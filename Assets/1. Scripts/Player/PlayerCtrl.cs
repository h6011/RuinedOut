using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyFunc;

public class PlayerCtrl : MonoBehaviour
{
    public static PlayerCtrl Instance;

    #region Managers

    private CanvasManager canvasManager_ins;
    private MouseSettings mouseSettings_ins;

    #endregion

    #region Stat ���� ������

    [HideInInspector] public float MaxHungry = 100f;
    [HideInInspector] public float MaxThirsty = 100f;
    [HideInInspector] public float MaxFatique = 100f;
    [HideInInspector] public float MaxHp = 100f;
    [HideInInspector] public float MaxStamina = 100f;

    [Header("Stat")]
    public float Hungry = 100f;
    public float Thirsty = 100f;
    public float Fatique = 100f;
    public float Hp = 100f;
    public float Stamina = 100f;

    [Header("Tick")]
    [SerializeField] private float R_HungryPerTick = 0.01f;
    [SerializeField] private float R_ThirstyPerTick = 0.03f;

    [Space]
    [SerializeField] private float R_Stats_Tick = 0.15f;
    private float R_Stats_TickCurrTime;


    #endregion

    #region �κ��丮 ���� ������

    /// <summary>
    /// �÷��̾��� �κ��丮
    /// </summary>
    [Space]
    public InventoryObject inventory;
    /// <summary>
    /// �÷��̾ ���� ������ ���⳪ ���� (ItemObject)
    /// </summary>
    public ItemObject Hand;
    /// <summary>
    /// Hand �� GameObject
    /// </summary>
    [HideInInspector] public GameObject HandObject;
    /// <summary>
    /// �÷��̾��� GameObject
    /// </summary>
    public GameObject PlayerObject;
    /// <summary>
    /// �÷��̾ �����ҽ� Hand�� GameObject�� �� GameObject
    /// </summary>
    public GameObject Grip;

    #endregion

    #region �ݴ� �Լ� ���� ������

    /// <summary>
    /// �÷��̾ �������� �ֿ�� �ִ� ����(Range)
    /// </summary>
    [Space]
    [Header("Pickup Stat")]
    [SerializeField] float PickUpRange = 3.5f;
    /// <summary>
    /// Pickup Raycast LayerMask
    /// </summary>
    [SerializeField] LayerMask PickupLayerMask;
    #endregion

    #region �÷��̾� ���� �Լ� ���� ������

    [Header("Attack Stat")]
    /// <summary>
    /// ���� �����̸� Ȯ�� �ϱ� ���� float
    /// </summary>
    private float AttackActionCurrTime;
    /// <summary>
    /// ���� Raycast �� ���� LayerMask
    /// </summary>
    [SerializeField] LayerMask attackLayerMask;

    #endregion

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        initManagerIns();
        AttackActionCurrTime = Time.time;
        R_Stats_TickCurrTime = Time.time;
    }

    private void HungryAndThirstyAction()
    {
        Hungry -= R_HungryPerTick;
        Thirsty -= R_ThirstyPerTick;

        Hungry = Mathf.Clamp(Hungry, 0f, MaxHungry);
        Thirsty = Mathf.Clamp(Thirsty, 0f, MaxThirsty);
    }

    private void HungryAndThirstyTickProcess()
    {
        if ( (Time.time - R_Stats_TickCurrTime) >= R_Stats_Tick )
        {
            R_Stats_TickCurrTime = Time.time;
            HungryAndThirstyAction();
        }
    }

    private void Update()
    {
        PickUpAction();
        EquipmentAttackAction();
        InputCheck1();
        HungryAndThirstyTickProcess();
    }
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }

    
    private void initManagerIns()
    {
        canvasManager_ins = CanvasManager.instance;
        mouseSettings_ins = MouseSettings.ins;
    }
    /// <summary>
    /// ���� �÷��̾ �������� �ֿ�� �ִ°Ÿ� ���� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <param name="Distance_"> ��� ���� </param>
    /// <returns></returns>
    private bool IsCanPickup(float Distance_)
    {
        if (Distance_ <= PickUpRange)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// �÷��̾ �������� �����ϰ� �ϴ� �Լ�
    /// </summary>
    /// <param name="itemObject"> ������ ������(ItemObject) </param>
    public void EquipEquipment(ItemObject _itemObject)
    {
        if (_itemObject != Hand) // ���� ���������� �ߺ� Ȯ��
        {
            if (HandObject != null) // ���� �ִ� Hand�� GameObject ���� Ȯ��
            {
                Destroy(HandObject); // ���� �ִ� Hand�� GameObject ���� ó��
            }
            GameObject ItemPrafab = ItemManager.Instance.GetItemPrefab(_itemObject, Grip.transform); // 1��Ī���� ���� HandObject �� ���� Prefab ������ Grip GameObject�� ��ġ
            ItemPrafab.transform.position = Grip.transform.position;
            ItemPrafab.transform.rotation = Grip.transform.rotation;
            HandObject = ItemPrafab;

        }
        Hand = _itemObject;
    }
    private void GetHungryAndThirstyFromFood(ItemObject _ItemObject)
    {
        Hungry += _ItemObject.Hungry;
        Thirsty += _ItemObject.Thirsty;
    }
    /// <summary>
    /// �÷��̾ ������ �԰� �ϴ� �Լ�
    /// </summary>
    /// <param name="_ItemObject"> ���� ����(ItemObject) </param>
    public void EatFood(ItemObject _ItemObject)
    {
        ItemManager.Instance.RemoveItemFromInventory(inventory, _ItemObject);
        GetHungryAndThirstyFromFood(_ItemObject);
    }



    /// <summary>
    /// �÷��̾��� �ݴ� �ý����� �Լ�
    /// </summary>
    private void PickUpAction()
    {
        OutlineMng.Instance.DisableAllOutlines();

        Ray ray = CameraSettings.instance.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, PickupLayerMask))
        {
            if (Checking.IsItItem(hit.transform))
            {
                if (IsCanPickup(hit.distance))
                {
                    canvasManager_ins.SetActive_MouseInfoText(true);
                    canvasManager_ins.Update_MouseInfoText(hit.transform.name);
                    if (Input.GetMouseButtonDown(0))
                    {
                        ItemManager.Instance.GetItemFromObject(PlayerCtrl.Instance, hit.transform.gameObject);
                    }

                    Outline findOutline = hit.transform.GetComponent<Outline>();
                    if (findOutline)
                    {
                        //findOutline.enabled = true;
                        Color color = findOutline.OutlineColor;
                        color.a = 1.0f;
                        findOutline.OutlineColor = color;
                    }
                }
                else
                {
                    canvasManager_ins.SetActive_MouseInfoText(false);
                }
            }
            else
            {
                canvasManager_ins.SetActive_MouseInfoText(false);
            }
        }
    }
    /// <summary>
    /// �÷��̾��� �Է� ���� �Լ�
    /// </summary>
    private void InputCheck1()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GameObject InventoryUI = CanvasManager.instance.InventoryUI;//CanvasManager.instance.GetUIFromName("Inventory");
            if (InventoryUI.activeSelf)
            {
                InventoryUI.SetActive(false);
                mouseSettings_ins.MouseLock(true);
                GameObject ItemInfoUI = CanvasManager.instance.GetUIFromName("ItemInfo");
                ItemInfoUI.SetActive(false);
            }
            else
            {
                InventoryUI.SetActive(true);
                mouseSettings_ins.MouseLock(false);
            }
        }
    }
    /// <summary>
    /// Equipment��  ���� �Լ�
    /// </summary>
    private void EquipmentAttackAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Hand != null)
            {
                if ((Time.time - AttackActionCurrTime) >= Hand.AttackDelay)
                {
                    if (Hand != null)
                    {
                        Attack();
                        AttackActionCurrTime = Time.time;
                    }
                }
                else
                {

                }
            }
        }
    }

    private void Attack()
    {
        Ray ray = CameraSettings.instance.MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Hand.Range, attackLayerMask))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                EnemyCtrl enemyCtrl = hit.transform.GetComponent<EnemyCtrl>();
                enemyCtrl.Hp -= Hand.Damage;
                enemyCtrl.GetAttackedEffect();
                EffectMng.Instance.MakeEffect1(EffectType.Block1, hit.point, 4, 0.5f, 1);
            }
        }
    }



}
