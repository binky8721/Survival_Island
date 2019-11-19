using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGameManager : MonoBehaviour {

    // 마우스 포인터로 사용할 텍스쳐 입력 받기
    public Texture2D BasicCursorTexture;
    public Texture2D EyesCursorTexture;

    private Vector2 hotSpot = Vector2.zero;

    //public bool bCursorChage = false;


    public static bool t_canPlayerMove = true; //플레이어의 움직임 제어.

    public static bool t_isOpenInventory = false; //인벤토리 활성화

    public static bool t_isPause = false; //일시 정지메뉴가 호출되면 true

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined; // 게임 창 밖으로 마우스가 안나감
    }

    void Start()
    {
        hotSpot.x = 10.0f;
        hotSpot.y = 33.0f;
        //Cursor.visible = false; //커서를 안보이게 함.
    }

    void Update()
    {
        /*
        * 오브젝트와 충돌시 eyes커서로 변경
        * 충돌이 아닐 때는 basic커서로 변경  
        */
        if (TutorialDialogue.instance == null) //튜토리얼이 꺼져있을때
        {
            if (Inventory.inventoryActivated || ControlDialogue.dialogActivated || GameManager.isPause || QuestUI.instance.isOpenCraftMenu)
            {
                Cursor.visible = true;
                Cursor.SetCursor(BasicCursorTexture, hotSpot, CursorMode.Auto);
                t_canPlayerMove = false;
            }
            else
            {
                if (Title.isTitle)
                    Cursor.visible = true;
                else
                    Cursor.visible = false;
                t_canPlayerMove = true;
            }
        }
        else if(TutorialDialogue.instance != null) //튜토리얼이 켜져있을때
        {
            //인벤토리,대사,일시정지 중 하나라도 작동시 커서를 키면서 플레이어가 움직이지 못하게한다.
            if (Inventory.inventoryActivated || TutorialControlDialogue.dialogActivated || t_isPause)
            {
                Cursor.visible = true;
                Cursor.SetCursor(BasicCursorTexture, hotSpot, CursorMode.Auto);
                t_canPlayerMove = false;
            }
            else
            {
                if (Title.isTitle)
                    Cursor.visible = true;
                else
                    Cursor.visible = false;
                t_canPlayerMove = true;
            }
        }
        //Debug.Log(t_isPause);
    }

 
}
