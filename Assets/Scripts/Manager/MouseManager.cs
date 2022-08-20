using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    public Texture2D point, doorway, attack, target, arrow;

    RaycastHit hitInfo;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
    private void Update()
    {
        SetCursorTexture();
        MouseController();
    }

    //设置鼠标图片
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,out hitInfo))
        {
            //切换鼠标图片
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Attackable":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Untagged":
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
    }


    void MouseController()
    {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null)
        {
            //点击是敌人则更改目标的Shaader，不是敌人则设为默认的Shader
            EventHandler.CallSelectedEnemyShaderChangeEvent(hitInfo.collider.gameObject);
            //点击地面触发事件
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
                EventHandler.CallGroundClickedEvent(hitInfo.point);
            //点击敌人触发事件
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                EventHandler.CallEnemyClickedEvent(hitInfo.collider.gameObject);
            //点击石头触发的事件
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
                EventHandler.CallEnemyClickedEvent(hitInfo.collider.gameObject);
            //点击地面触发事件
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
                EventHandler.CallGroundClickedEvent(hitInfo.point);

        }
    }
}
