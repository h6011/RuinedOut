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

    #region Stat 관련 변수들

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

    #region 인벤토리 관련 변수들

    /// <summary>
    /// 플레이어의 인벤토리
    /// </summary>
    [Space]
    public InventoryObject inventory;
    /// <summary>
    /// 플레이어가 현재 장착한 무기나 음식 (ItemObject)
    /// </summary>
    public ItemObject Hand;
    /// <summary>
    /// Hand 의 GameObject
    /// </summary>
    [HideInInspector] public GameObject HandObject;
    /// <summary>
    /// 플레이어의 GameObject
    /// </summary>
    public GameObject PlayerObject;
    /// <summary>
    /// 플레이어가 장착할시 Hand의 GameObject가 들어갈 GameObject
    /// </summary>
    public GameObject Grip;

    #endregion

    #region 줍는 함수 관련 변수들

    /// <summary>
    /// 플레이어가 아이템을 주울수 있는 범위(Range)
    /// </summary>
    [Space]
    [Header("Pickup Stat")]
    [SerializeField] float PickUpRange = 3.5f;
    /// <summary>
    /// Pickup Raycast LayerMask
    /// </summary>
    [SerializeField] LayerMask PickupLayerMask;
    #endregion

    #region 플레이어 공격 함수 관련 변수들

    [Header("Attack Stat")]
    /// <summary>
    /// 공격 딜레이를 확인 하기 위한 float
    /// </summary>
    private float AttackActionCurrTime;
    /// <summary>
    /// 공격 Raycast 를 위한 LayerMask
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
    /// 현재 플레이어가 아이템을 주울수 있는거리 인지 확인하는 함수
    /// </summary>
    /// <param name="Distance_"> 허용 범위 </param>
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
    /// 플레이어가 아이템을 장착하게 하는 함수
    /// </summary>
    /// <param name="itemObject"> 장착할 아이템(ItemObject) </param>
    public void EquipEquipment(ItemObject _itemObject)
    {
        if (_itemObject != Hand) // 같은 아이템인지 중복 확인
        {
            if (HandObject != null) // 전에 있던 Hand의 GameObject 유무 확인
            {
                Destroy(HandObject); // 전에 있던 Hand의 GameObject 삭제 처리
            }
            GameObject ItemPrafab = ItemManager.Instance.GetItemPrefab(_itemObject, Grip.transform); // 1인칭에서 보일 HandObject 와 같은 Prefab 복사후 Grip GameObject에 위치
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
    /// 플레이어가 음식을 먹게 하는 함수
    /// </summary>
    /// <param name="_ItemObject"> 먹을 음식(ItemObject) </param>
    public void EatFood(ItemObject _ItemObject)
    {
        ItemManager.Instance.RemoveItemFromInventory(inventory, _ItemObject);
        GetHungryAndThirstyFromFood(_ItemObject);
    }



    /// <summary>
    /// 플레이어의 줍는 시스템의 함수
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
    /// 플레이어의 입력 관련 함수
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
    /// Equipment의  공격 함수
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
