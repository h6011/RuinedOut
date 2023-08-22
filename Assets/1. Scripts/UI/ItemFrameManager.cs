using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using MyFunc;
using UnityEngine.UI;
using System;

public class ItemFrameManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text NameText;
    [SerializeField] private TMP_Text AmountText;
    [SerializeField] private Image Img;
    public ItemObject targetItemObject;

    private bool IsEnter = false;

    public void ChangeNameText(string text)
    {
        NameText.text = text;
    }
    public void ChangeAmountText(string text)
    {
        AmountText.text = string.Format(" {0}", text);
    }
    public void ChangeImg(Sprite sprite)
    {
        Img.sprite = sprite;
    }

    public void ChangeImgColor(Color color)
    {
        Img.color = color;
    }

    public void OnLeftClicked()
    {

    }
    
    public void OnRightClicked()
    {
        GameObject ItemInfoUI = CanvasManager.instance.GetUIFromName("ItemInfo");
        if (ItemInfoUI.activeSelf)
        {
            ItemInfoUI.SetActive(false);
        }
        else
        {
            ItemInfoUI.SetActive(true);
            ItemInfoUI.transform.position = Input.mousePosition + new Vector3(1, -1);
            ItemInfoUI _itemInfoUI = ItemInfoUI.GetComponent<ItemInfoUI>();
            _itemInfoUI.itemObject = targetItemObject;
            _itemInfoUI.ChangeItemNameText(targetItemObject.name);

            Transform StatUI_ = ItemInfoUI.transform.Find("Stat");

            Sprite imgsprite = Resources.Load<Sprite>("Sprites/" + targetItemObject.name);
            if (imgsprite)
            {
                _itemInfoUI.ChangeImg(imgsprite);
            }
            else
            {
                ItemInfoUI.transform.Find("Img").gameObject.SetActive(false);
            }

            string[] list1 = new string[] {};

            if (targetItemObject.ItemType == ItemType.Food)
            {
                _itemInfoUI.ChangeBtnText("Eat");
                list1 = new string[] { "Hungry", "Thirsty" };
            }
            else if (targetItemObject.ItemType == ItemType.Equipment)
            {
                _itemInfoUI.ChangeBtnText("Equip");
                list1 = new string[] { "Damage", "Range" , "AttackDelay"};
            }

            for (int i = 0; i < StatUI_.childCount; i++)
            {
                Transform v = StatUI_.GetChild(i);
                bool Included = false;

                for (int i2 = 0; i2 < list1.Length; i2++)
                {
                    if (v.name == list1[i2])
                    {
                        Included = true;
                        break;
                    }
                }

                if (Included)
                {
                    Transform CStat = StatUI_.Find(v.name);
                    TMP_Text Text_ = CStat.Find("Text").GetComponent<TMP_Text>();
                        
                    CStat.gameObject.SetActive(true);
                    Text_.text = targetItemObject.GetStatFromName(v.name).ToString();

                }
                else
                {
                    StatUI_.Find(v.name).gameObject.SetActive(false);
                }
            } // Equipment, Food 일떄 특정 텍스트만 보이게하는

        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsEnter = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsEnter = true;
    }

    private void Update()
    {
        if (IsEnter && Input.GetMouseButtonDown(1))
        {
            OnRightClicked();
        }
    }
}
