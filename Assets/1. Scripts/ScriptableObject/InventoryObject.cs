using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].AddAmount(_amount);
                CanvasManager.instance.SetInventorySlotAmountUI(_item, Container[i].amount);
                hasItem = true;
                break;
            }
        }
        if (!hasItem)
        {
            InventorySlot New1 = new InventorySlot(_item, _amount);
            Container.Add(New1);
            CanvasManager.instance.Add_NewInventorySlotUI(_item);
        }
    }
    public void RemoveItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                Container[i].RemoveAmount(_amount);
                hasItem = true;
                if (Container[i].GetAmount() <= 0)
                {
                    Container.Remove(Container[i]);
                    CanvasManager.instance.Remove_InventorySlotUI(_item);
                }
                break;
            }
        }
        if (!hasItem)
        {
            Debug.Log(_item.name + " is not existed");
        }
    }




}


[Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;
    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void AddAmount(int value)
    {
        amount += value;
    }
    public void RemoveAmount(int value)
    {
        amount -= value;
    }

    public int GetAmount()
    {
        return amount;
    }
}