using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public GameObject ItemManagerObj;

    private void Awake()
    {
        Instance = this;
    }

    public bool IsItItem(GameObject target)
    {
        if (target.CompareTag("Item"))
        {
            return true;
        }
        return false;
    }

    public bool IsItItem(Transform target)
    {
        if (target.CompareTag("Item"))
        {
            return true;
        }
        return false;
    }

    public void GetItem(PlayerCtrl playerCtrl, ItemObject itemObject, int amount)
    {
        playerCtrl.inventory.AddItem(itemObject, amount);
    }

    public void GetItemFromObject(PlayerCtrl playerCtrl, GameObject _Item)
    {
        ItemObject Target_ItemObject = _Item.transform.GetComponent<Item>().item;
        GetItem(playerCtrl, Target_ItemObject, 1);
        Destroy(_Item.transform.gameObject);
    }

    public void RemoveItemFromInventory(InventoryObject inventoryObject, ItemObject _Target_ItemObject)
    {
        inventoryObject.RemoveItem(_Target_ItemObject, 1);

    }

    public GameObject GetItemPrefab(string name)
    {
        GameObject[] New1 = Resources.LoadAll<GameObject>("Prefab/Item");
        foreach (var item in New1)
        {
            if (item.name.Equals(name))
            {
                GameObject NewItem = Instantiate<GameObject>(item, transform);
                return NewItem;
            }
        }
        return null;
    }
    public GameObject GetItemPrefab(ItemObject itemObject, Transform parent)
    {
        GameObject[] New1 = Resources.LoadAll<GameObject>("Prefab/Item");
        foreach (var item in New1)
        {
            Item _item = item.GetComponent<Item>();
            if (_item && _item.item == itemObject)
            {
                GameObject NewItem = Instantiate<GameObject>(item, parent);
                return NewItem;
            }
        }
        return null;
    }

    public void SpawnItem(string name)
    {
        GameObject[] New1 = Resources.LoadAll<GameObject>("Prefab/Item");
        foreach (var item in New1)
        {
            if (item.name.Equals(name))
            {
                GameObject NewItem = Instantiate<GameObject>(item, transform);
                break;
            }
        }
    }

    


}
