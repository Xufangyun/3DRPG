using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public InventoryData bagData;
    public GameObject slotGrid;
    public GameObject emptySlot;
    public Text itemInfo;
    public GameObject myBag;
    private List<Slot> slotList = new List<Slot>();


    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RefreshInventory()
    {
        slotList.Clear();
        for (int i = 0; i < slotGrid.transform.childCount; i++)
        {
            if (slotGrid.transform.childCount == 0)
                break;
            Destroy(slotGrid.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < bagData.itemList.Count; i++)
        {
            //CreateNewItem(myBag.itemList[i]);
            slotList.Add(Instantiate(emptySlot).GetComponent<Slot>());
            slotList[i].gameObject.transform.SetParent(slotGrid.transform);
            slotList[i].slotID = i;
            slotList[i].InitSlotUI(bagData.itemList[i]);
        }
    }

    public void OpenorCloseBag(bool isActive)
    {
        myBag.SetActive(isActive);
        RefreshInventory();
    }

    public void UpdateItemInfo(string itemDescription)
    {
        itemInfo.text = itemDescription;
    }

}