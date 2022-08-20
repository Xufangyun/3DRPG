using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New ItemData",menuName ="Inventory/New ItemData")]
public class ItemData_SO : ScriptableObject
{
    public string itemName;

    public Sprite itemSprite;

    public int itemHeld;

    public string itemInfo;

    public bool Equip;
}
