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
            _itemInfoUI.ShowItemInfo(targetItemObject);
            _itemInfoUI.itemObject = targetItemObject;

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
