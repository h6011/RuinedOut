using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    public ItemObject itemObject;
    [SerializeField] TMP_Text itemNameText;
    [SerializeField] TMP_Text btnText;

    public void OnBtn1LeftClicked()
    {
        if (itemObject.ItemType == ItemType.Food)
        {
            PlayerCtrl.Instance.EatFood(itemObject);
        }
        else if (itemObject.ItemType == ItemType.Equipment)
        {
            PlayerCtrl.Instance.EquipEquipment(itemObject);
        }
        CanvasManager.instance.GetUIFromName("ItemInfo").SetActive(false);
    }

    public void ChangeItemNameText(string _text)
    {
        itemNameText.text = _text;
    }

    public void ChangeBtnText(string _text)
    {
        btnText.text = _text;
    }

}
