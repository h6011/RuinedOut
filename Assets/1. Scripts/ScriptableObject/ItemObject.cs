using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ItemType{
    Default,
    Food,
    Equipment
}
[CreateAssetMenu(fileName ="New Item", menuName = "Item System/Item")]


public class ItemObject : ScriptableObject
{
    public string ItemName;
    public ItemType ItemType;
    public float Weight;
    public GameObject Prefab;

    [Space]
    [TextArea(1, 3)]
    public string Description;

    [Header("Food Stat")]
    public float Hungry;
    public float Thirsty;

    [Header("Equipment Stat")]
    public float Damage;
    public float Range;
    public float Durability;
    public float AttackDelay;

    public float GetStatFromName(string Name_)
    {
        switch (Name_)
        {
            case "Hungry":
                return Hungry;
            case "Thirsty":
                return Thirsty;
            case "Damage":
                return Damage;
            case "Range":
                return Range;
            case "Durability":
                return Durability;
            case "AttackDelay":
                return AttackDelay;
            default:
                return -1111;
        };
    }


}
