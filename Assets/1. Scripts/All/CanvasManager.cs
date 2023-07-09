using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public List<GameObject> UI_GameObject = new List<GameObject>();
    public List<TMP_Text> UI_TMP_Text = new List<TMP_Text>();

    public TMP_Text MouseInfoText;

    [Header("UIs")]
    public GameObject MouseInfoUI;
    public GameObject InventoryUI;
    public GameObject ItemInfoUI;
    public GameObject StatInfoUI;



  

    private void Awake()
    {
        instance = this;
        MouseInfoText = GetTMP_TextFromName("MouseInfo");
    }

    private void Start()
    {
        initUI();
    }

    private void Update()
    {
        ShowStats();
    }
    

    private Transform GetFrontBarFromStatUI(string Name_)
    {
        return StatInfoUI.transform.Find(Name_).Find("BackBar").Find("FrontBarMask");
    }
    
    private void ShowStats()
    {
        GetFrontBarFromStatUI("Hp").GetComponent<Image>().fillAmount = Mathf.Clamp01(PlayerCtrl.Instance.Hp / PlayerCtrl.Instance.MaxHp);
        GetFrontBarFromStatUI("Hungry").GetComponent<Image>().fillAmount = Mathf.Clamp01(PlayerCtrl.Instance.Hungry / PlayerCtrl.Instance.MaxHungry);
        GetFrontBarFromStatUI("Thirsty").GetComponent<Image>().fillAmount = Mathf.Clamp01(PlayerCtrl.Instance.Thirsty / PlayerCtrl.Instance.MaxThirsty);
        GetFrontBarFromStatUI("Fatique").GetComponent<Image>().fillAmount = Mathf.Clamp01(PlayerCtrl.Instance.Fatique / PlayerCtrl.Instance.MaxFatique);
        GetFrontBarFromStatUI("Stamina").GetComponent<Image>().fillAmount = Mathf.Clamp01(PlayerCtrl.Instance.Stamina / PlayerCtrl.Instance.MaxStamina);



    }

    private void initUI()
    {
        GetUIFromName("MouseInfo").SetActive(false);
        MouseInfoUI.SetActive(false);
        InventoryUI.SetActive(false);
        ItemInfoUI.SetActive(false);
    }

    public void Update_MouseInfoText(string name)
    {
        TMP_Text New = MouseInfoText;
        New.text = name;
    }

    public void SetActive_MouseInfoText(bool boolen)
    {
        GameObject New = MouseInfoText.gameObject;
        New.SetActive(boolen);
    }

    public GameObject GetUIFromName(string name)
    {
        for (int iNum = 0; iNum < UI_GameObject.Count; iNum++)
        {
            if (UI_GameObject[iNum].name.Equals(name))
            {
                return UI_GameObject[iNum];
            }
        }
        return null;
    }
    public TMP_Text GetTMP_TextFromName(string name)
    {
        for (int iNum = 0; iNum < UI_TMP_Text.Count; iNum++)
        {
            if (UI_TMP_Text[iNum].name.Equals(name))
            {
                return UI_TMP_Text[iNum];
            }
        }
        return null;
    }

    public void Add_NewInventorySlotUI(ItemObject itemObject_)
    {
        GameObject Parent1 = GetUIFromName("Inventory").transform.Find("Right").gameObject;
        //GameObject New1 = Instantiate(Resources.Load<GameObject>("Prefab/UI/ItemFrame"), Parent1.transform);
        GameObject New1 = PoolingMng.Instance.CreateObj(PoolingObj.ItemFrame, Parent1.transform);
        New1.name = itemObject_.name;

        ItemFrameManager New2 = New1.GetComponent<ItemFrameManager>();
        New2.ChangeNameText(string.Format("{0}", itemObject_.name));
        New2.ChangeAmountText("1");
        New2.targetItemObject = itemObject_;
    }

    public void Remove_InventorySlotUI(ItemObject itemObject_)
    {
        GameObject Parent1 = GetUIFromName("Inventory").transform.Find("Right").gameObject;
        Transform Find1 = Parent1.transform.Find(itemObject_.name);
        if (Find1)
        {
            PoolingMng.Instance.RemoveObj(Find1.gameObject, PoolingObj.ItemFrame);
            //Debug.Log(Find1.gameObject);
            //Destroy(Find1.gameObject);
        }
    }

    public void SetInventorySlotAmountUI(ItemObject item, int amount)
    {
        GameObject Parent1 = GetUIFromName("Inventory").transform.Find("Right").gameObject;
        Transform FindSlot = Parent1.transform.Find(item.name);
        ItemFrameManager New2 = FindSlot.GetComponent<ItemFrameManager>();
        New2.ChangeAmountText(amount.ToString());
    }



}
