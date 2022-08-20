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

    //�������ͼƬ
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray,out hitInfo))
        {
            //�л����ͼƬ
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
            //����ǵ��������Ŀ���Shaader�����ǵ�������ΪĬ�ϵ�Shader
            EventHandler.CallSelectedEnemyShaderChangeEvent(hitInfo.collider.gameObject);
            //������津���¼�
            if (hitInfo.collider.gameObject.CompareTag("Ground"))
                EventHandler.CallGroundClickedEvent(hitInfo.point);
            //������˴����¼�
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                EventHandler.CallEnemyClickedEvent(hitInfo.collider.gameObject);
            //���ʯͷ�������¼�
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
                EventHandler.CallEnemyClickedEvent(hitInfo.collider.gameObject);
            //������津���¼�
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
                EventHandler.CallGroundClickedEvent(hitInfo.point);

        }
    }
}
