using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [HideInInspector]
    public ItemData_SO item;

    [HideInInspector]
    public int slotID;

    public Image slotImage;
    public Text slotNum;
    public GameObject itemInSlot;

    public void InitSlotUI(ItemData_SO item)
    {
        if (item == null)
        {
            itemInSlot.SetActive(false);
            return;
        }
        this.item = item;

        slotImage.sprite = item.itemSprite;
        slotNum.text = item.itemHeld.ToString();
    }

    public void ItemOnClicked()
    {
        InventoryManager.Instance.UpdateItemInfo(item.itemInfo);
        InventoryManager.Instance.SwitchWeapon(item.prefab);
    }
}
