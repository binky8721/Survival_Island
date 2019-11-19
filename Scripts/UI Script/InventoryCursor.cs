using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCursor : MonoBehaviour {

    // 마우스 포인터로 사용할 텍스쳐 입력 받기
    public Texture2D BasicCursorTexture;
    public Texture2D EyesCursorTexture;

    private Vector2 hotSpot = Vector2.zero;

    //public bool bCursorChage = false;

    void Start()
    {
        hotSpot.x = 10.0f;
        hotSpot.y = 33.0f;
    }

    void Update()
    {
        /*
         * 오브젝트와 충돌시 eyes커서로 변경
         * 충돌이 아닐 때는 basic커서로 변경  
         */
        //if (bCursorChage)
        if (Inventory.inventoryActivated || ControlDialogue.dialogActivated) //인벤토리 작동하거나 대화작동시 커서킴
        {
            Cursor.visible = true;
            Cursor.SetCursor(BasicCursorTexture, hotSpot, CursorMode.Auto);
        }
        else
        {
            if (Title.isTitle)
                Cursor.visible = true;
            else
                Cursor.visible = false;
        }
        //else
        //    Cursor.SetCursor(BasicCursorTexture, hotSpot, CursorMode.Auto);
    }

    /*
    public void PointerChange(bool active)
    {
        bCursorChage = active;
    }
    */
}
