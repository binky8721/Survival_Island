using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // 마우스 포인터로 사용할 텍스쳐 입력 받기
    public Texture2D BasicCursorTexture;
    public Texture2D EyesCursorTexture;

    private Vector2 hotSpot = Vector2.zero;

    //public bool bCursorChage = false;
    
        
    public static bool canPlayerMove = true; //플레이어의 움직임 제어.

    public static bool isOpenInventory = false; //인벤토리 활성화

    public static bool isPause = false; //일시 정지메뉴가 호출되면 true

    void Awake()
    {
        //대화창 끄고나서는 lock으로 잠그고 대화창 끄고나서는 풀기.
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Start ()
    {
        hotSpot.x = 10.0f;
        hotSpot.y = 33.0f;
        //Cursor.visible = false; //커서를 안보이게 함.
	}
	
	void Update ()
    {
        /*
        * 오브젝트와 충돌시 eyes커서로 변경
        * 충돌이 아닐 때는 basic커서로 변경  
        */
        if (TutorialDialogue.instance == null)
        {
            if (Inventory.inventoryActivated || ControlDialogue.dialogActivated || isPause || QuestUI.instance.isOpenCraftMenu || CraftManual.craftActivated)
            {
                //Cursor.lockState = CursorLockMode.None; 
                Cursor.visible = true;
                Cursor.SetCursor(BasicCursorTexture, hotSpot, CursorMode.Auto);

                if (CraftManual.craftActivated)
                    canPlayerMove = true;
                else
                    canPlayerMove = false;
            }
            else
            {
                if (Title.isTitle)
                    Cursor.visible = true;
                else
                    Cursor.visible = false;
                canPlayerMove = true;
            }
        }
        else if(TutorialDialogue.instance != null)
        {
            if (Inventory.inventoryActivated || TutorialControlDialogue.dialogActivated || TutorialGameManager.t_isPause)
            {
                //Cursor.lockState = CursorLockMode.None; 
                Cursor.visible = true;
                Cursor.SetCursor(BasicCursorTexture, hotSpot, CursorMode.Auto);
                canPlayerMove = false;
            }
            else
            {
                if (Title.isTitle)
                    Cursor.visible = true;
                else
                    Cursor.visible = false;
                canPlayerMove = true;
            }
        }
        
	}

}
