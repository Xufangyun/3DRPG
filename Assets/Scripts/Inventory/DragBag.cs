using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragBag : MonoBehaviour,IDragHandler
{
    RectTransform currentTrans;
    void Start()
    {
        currentTrans = GetComponent<RectTransform>();
    }


    public void OnDrag(PointerEventData eventData)
    {
        currentTrans.anchoredPosition += eventData.delta;
    }
}
