using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    public ItemObject itemObject;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text btnText;
    [SerializeField] private Image img;

    string[] statsForFood = new string[] { "Hungry", "Thirsty" };
    string[] statsForEquipment = new string[] { "Damage", "Range", "AttackDelay" };

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

    public void ChangeImg(Sprite sprite)
    {
        img.sprite = sprite;
    }

    public void ClickedBackgroundBorder()
    {
        Debug.Log("ClickedBackgroundBorder");
        itemObject = null;
        CanvasManager.instance.ItemInfoUI.SetActive(false);
    }

    public void ShowItemInfo(ItemObject targetitemObject)
    {
        ChangeItemNameText(targetitemObject.name);

        Transform StatUI_ = transform.Find("Stat");

        Sprite imgsprite = Resources.Load<Sprite>("Sprites/" + targetitemObject.name);
        if (imgsprite)
        {
            ChangeImg(imgsprite);
            transform.Find("Img").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Img").gameObject.SetActive(false);
        }

        string[] list1 = new string[] {};

        if (targetitemObject.ItemType == ItemType.Food)
        {
            ChangeBtnText("Eat");
            list1 = statsForFood;
        }
        else if (targetitemObject.ItemType == ItemType.Equipment)
        {
            ChangeBtnText("Equip");
            list1 = statsForEquipment;
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
                Text_.text = targetitemObject.GetStatFromName(v.name).ToString();

            }
            else
            {
                StatUI_.Find(v.name).gameObject.SetActive(false);
            }
        } // Equipment, Food 일떄 특정 텍스트만 보이게하는


    }

}
