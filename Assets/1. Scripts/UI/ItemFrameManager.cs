using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using MyFunc;

public class ItemFrameManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Text NameText;
    [SerializeField] TMP_Text AmountText;
    public ItemObject targetItemObject;

    private bool IsEnter = false;

    public void ChangeNameText(string text)
    {
        NameText.text = text;
    }
    public void ChangeAmountText(string text)
    {
        AmountText.text = text;
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
            if (targetItemObject.ItemType == ItemType.Food)
            {
                _itemInfoUI.ChangeBtnText("Eat");
            }
            else if (targetItemObject.ItemType == ItemType.Equipment)
            {
                _itemInfoUI.ChangeBtnText("Equip");
            }
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
