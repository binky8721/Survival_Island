using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Craft
{
    public string craftName; //이름
    public Sprite craftImage; //이미지
    public string craftDesc; //설명
    public string[] craftNeedItem; //필요한 아이템
    public int[] craftNeedItemCount; //필요한 아이템의 개수
    public GameObject go_Prefab; //실제 설치될 프리팹.
    public GameObject go_PreviewPrefab; //미리보기 프리팹.
}

public class CraftManual : MonoBehaviour {

    //상태변수
    public static bool craftActivated = false;
    private bool isPreviewActivated = false;
    public static bool isMakeRaft = false;

    [SerializeField]
    private GameObject go_BaseUI; //기본 베이스 UI

    private int tabNumber = 0;
    private int page = 1;
    private int selectedSlotNumber;
    private Craft[] craft_SelectedTab; //선택한 탭에 들어있는 크래프트 정보들을 담을곳

    [SerializeField]
    private Craft[] craft_fire; //모닥불용 탭.
    [SerializeField]
    private Craft[] craft_build; //건축용 탭.

    private GameObject go_Preview; //미리보기 프리팹을 담을 변수.
    private GameObject go_Prefab; //실제 생성될 프리팹을 담을 변수.

    [SerializeField]
    private Transform tf_Player; //플레이어 위치

    //Raycast 필요 변수 선언
    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private float buildingRange;

    //필요한 UI Slot 요소
    [SerializeField]
    private GameObject[] go_Slots;
    [SerializeField]
    private Image[] image_Slot;
    [SerializeField]
    private Text[] text_SlotName;
    [SerializeField]
    private Text[] text_SlotDesc;
    [SerializeField]
    private Text[] text_SlotNeedItem;

    //필요한 컴포넌트
    private Inventory theInventory;

    void Start()
    {
        theInventory = FindObjectOfType<Inventory>();
        tabNumber = 0;
        page = 1;
        TabSlotSetting(craft_fire); //초기 탭정보 파이어탭으로
    }

    public void TabSetting(int _tabNumber)
    {
        tabNumber = _tabNumber;
        page = 1; //페이지는 1로 초기화
        switch(tabNumber)
        {
            case 0:
                TabSlotSetting(craft_fire);// 불 세팅
                break;
            case 1:
                TabSlotSetting(craft_build);//건축 세팅
                break;
        }
    }

    public void RightPageSetting()
    {
        if (page < (craft_SelectedTab.Length / go_Slots.Length) + 1)//최대페이지가 넘는지 확인
            page++;
        else
            page = 1;

        TabSlotSetting(craft_SelectedTab);
    }

    public void LeftPageSetting()
    {
        if (page != 1) //1페이지가 아닐때 1씩감소
            page--;
        else
            page = (craft_SelectedTab.Length / go_Slots.Length) + 1; //1페이지에서 왼쪽으로 갈경우 최대페이지로 이동

        TabSlotSetting(craft_SelectedTab);
        
    }

    private void TabSlotSetting(Craft[] _craft_tab)
    {
        ClearSlot(); //기존 탭정보 클리어
        craft_SelectedTab = _craft_tab;

        int startSlotNumber = (page - 1) * go_Slots.Length; // 페이지가 늘어남에따라 4의 배수로 늘어난다.

        for (int i = startSlotNumber; i < craft_SelectedTab.Length; i++)
        {
            if (i == page * go_Slots.Length)
                break;

            go_Slots[i - startSlotNumber].SetActive(true);

            image_Slot[i - startSlotNumber].sprite = craft_SelectedTab[i].craftImage;
            text_SlotName[i - startSlotNumber].text = craft_SelectedTab[i].craftName;
            text_SlotDesc[i - startSlotNumber].text = craft_SelectedTab[i].craftDesc;

            //하나의 제작에 여러아이템이 필요할수 있으므로 한번더 for문을 돌린다
            for(int x =0; x< craft_SelectedTab[i].craftNeedItem.Length ; x++)
            {
                text_SlotNeedItem[i - startSlotNumber].text += craft_SelectedTab[i].craftNeedItem[x];
                text_SlotNeedItem[i - startSlotNumber].text += " x " + craft_SelectedTab[i].craftNeedItemCount[x] + "\n";
            }
        }
    }
    //재료개수체크
    private bool CheckIngredient()
    {
        //필요한 아이템 갯수만큼 있는지 검사
        for (int i = 0; i < craft_SelectedTab[selectedSlotNumber].craftNeedItem.Length; i++) //아이템 종류별 검사 for문
        {
            if(theInventory.GetItemCount(craft_SelectedTab[selectedSlotNumber].craftNeedItem[i])<craft_SelectedTab[selectedSlotNumber].craftNeedItemCount[i]) //선택된 탭의 아이템 정보를 넘겨서 그아이템이 있는지 확인
            return false;
        }

        return true;
    }

    private void UseIngredient()
    {
        for (int i = 0; i < craft_SelectedTab[selectedSlotNumber].craftNeedItem.Length; i++)
        {
            theInventory.SetItemCount(craft_SelectedTab[selectedSlotNumber].craftNeedItem[i], craft_SelectedTab[selectedSlotNumber].craftNeedItemCount[i]);
        }
    }

    public void SlotClick(int _slotNumber)
    {
        selectedSlotNumber = _slotNumber + (page - 1) * go_Slots.Length;

        if (!CheckIngredient()) //재료가 없다면 프리뷰생성을 하지 않음
            return;
        go_Preview = Instantiate(craft_SelectedTab[selectedSlotNumber].go_PreviewPrefab, tf_Player.position + tf_Player.forward, Quaternion.identity);
        go_Prefab = craft_SelectedTab[selectedSlotNumber].go_Prefab;

        /*
        switch (selectedSlotNumber)
        {
            //모닥불
            case 0:
                Debug.Log("남은부싯돌개수" + QuestUI.instance.currentStoneNumber);
                if (QuestUI.instance.currentStoneNumber < 3)
                {
                    Debug.Log("모닥불 건설불가능");
                    Destroy(go_Preview);
                    craftActivated = false;
                    isPreviewActivated = false;
                    go_Prefab = null;
                    go_Preview = null;
                    go_BaseUI.SetActive(false);
                    return;
                }
                else
                {
                    QuestUI.instance.currentStoneNumber -= 3;
                    break;
                }
            //나무기둥
            case 1:
                Debug.Log("남은나무개수" + QuestUI.instance.currentWoodNumber);
                if (QuestUI.instance.currentWoodNumber < 2)
                {
                    Debug.Log("건설불가능");
                    Destroy(go_Preview);
                    craftActivated = false;
                    isPreviewActivated = false;
                    go_Prefab = null;
                    go_Preview = null;
                    go_BaseUI.SetActive(false);
                    return;
                }
                else
                {
                    QuestUI.instance.currentWoodNumber -= 2;
                    break;
                }
            //나무토대
            case 2:
                Debug.Log("남은나무개수" + QuestUI.instance.currentWoodNumber);
                if (QuestUI.instance.currentWoodNumber < 5)
                {
                    Debug.Log("건설불가능");
                    Destroy(go_Preview);
                    craftActivated = false;
                    isPreviewActivated = false;
                    go_Prefab = null;
                    go_Preview = null;
                    go_BaseUI.SetActive(false);
                    return;
                }
                else
                {
                    QuestUI.instance.currentWoodNumber -= 5;
                    break;
                }
            //구조신호
            case 3:
                if (QuestUI.instance.currentWoodNumber < 5 && QuestUI.instance.currentStoneNumber < 3)
                {
                    Debug.Log("건설불가능");
                    Destroy(go_Preview);
                    craftActivated = false;
                    isPreviewActivated = false;
                    go_Prefab = null;
                    go_Preview = null;
                    go_BaseUI.SetActive(false);
                    return;
                }
                else
                {
                    QuestUI.instance.currentWoodNumber -= 5;
                    QuestUI.instance.currentStoneNumber -= 3;
                    break;
                }
            default:
                break;
        }
        */
        isPreviewActivated = true;
        go_BaseUI.SetActive(false);
    }

    private void ClearSlot()
    {
        for (int i = 0; i < go_Slots.Length; i++)
        {
            image_Slot[i].sprite = null;
            text_SlotDesc[i].text = "";
            text_SlotName[i].text = "";
            text_SlotNeedItem[i].text = "";
            go_Slots[i].SetActive(false);
        }
    }

    void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isPreviewActivated)
            CraftWindow();
        if (isPreviewActivated)
            PreviewPositionUpdate();
        if (Input.GetButtonDown("Fire1"))
            Building();
        if (Input.GetKeyDown(KeyCode.Escape))
            BuildingCancel();
	}

    private void PreviewPositionUpdate()
    {
        if(Physics.Raycast(tf_Player.position,tf_Player.forward,out hitInfo, buildingRange, layerMask))
        {
            if(hitInfo.transform !=null)
            {
                Vector3 _location = hitInfo.point; //빛이 맞은 위치값을 저장

                _location.Set(Mathf.Round(_location.x), Mathf.Round(_location.y / 0.1f) * 0.1f, Mathf.Round(_location.z));
                go_Preview.transform.position = _location; //프리뷰위치값에 다시 대입

                //q,e 누를시 90도씩 회전
                if (Input.GetKeyDown(KeyCode.Q))
                    go_Preview.transform.Rotate(0, -90f, 0f);
                if (Input.GetKeyDown(KeyCode.E))
                    go_Preview.transform.Rotate(0, 90f, 0f);
            }
        }
    }

    private void Building()
    {
        //if(isPreviewActivated)//&& go_Preview.GetComponent<PreviewObject>().isBuildable())
        if(isPreviewActivated && go_Preview.GetComponent<PreviewObject>().isBuildable())
        {
            UseIngredient(); //건축하기전 재료소모코드

            Instantiate(go_Prefab, go_Preview.transform.position, go_Preview.transform.rotation);
            Destroy(go_Preview);
            craftActivated = false;
            isPreviewActivated = false;

            //하드코딩 모닥불 퀘스트//
            if (go_Prefab.name == "FireWood")
                QuestUI.instance.finalQuestCurrentFireWoodNumber++;
            //하드코딩 마지막 구조신호 제작//
            if (go_Prefab.name == "campfire")
                EventManager.instance.eventNumber = 17;
            if (go_Prefab.name == "raft")
            {
                EventManager.instance.eventNumber = 19;
                //EventManager.instance.GameClear();
            }
            go_Prefab = null;
            go_Preview = null;



        }
    }

    private void BuildingCancel()
    {
        if (isPreviewActivated)
            Destroy(go_Preview);

        craftActivated = false;
        isPreviewActivated = false;
        go_Prefab = null;
        go_Preview = null;
        go_BaseUI.SetActive(false);
    }

    private void CraftWindow()
    {
        if (!craftActivated)
            OpenCraftWindow();
        else
            CloseCraftWindow();

    }

    private void OpenCraftWindow()
    {
        craftActivated = true;
        go_BaseUI.SetActive(true);
    }

    private void CloseCraftWindow()
    {
        craftActivated = false;
        go_BaseUI.SetActive(false);
    }
}
