using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New InventoryData",menuName ="Inventory/New InventoryData")]
public class InventoryData : ScriptableObject
{
    public List<ItemData_SO> itemList = new List<ItemData_SO>();
}
