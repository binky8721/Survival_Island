using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour {

    //필요한 컴포넌트
    [SerializeField]
    private Text questNameText;
    [SerializeField]
    private GameObject firstQuest;
    [SerializeField]
    private Text[] firstQuestTexts;
    [SerializeField]
    private GameObject SecondQuest;
    [SerializeField]
    private Text SecondQuestText;
    [SerializeField]
    private GameObject FinalQuest;
    [SerializeField]
    private Text[] FinalQuestTexts;

    //임시 게임클리어 Text
    [SerializeField]
    private GameObject gameClearText;
    //임시 모닥불 아이템 프리펩
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject goFireWoodPrefab;
    [SerializeField]
    private GameObject craftMenu;

    //자기 자신 반환 인스턴스 (다른곳에서 자주쓰이므로 스테틱으로 썼음)
    public static QuestUI instance;

    public int currentQuestNumber; // 1,2,3 :각각 퀘스트넘버 0: 아무런 퀘스트 없음
    public int savedQuestNumber; //퀘스트 넘버 저장용

    public int firstQuestCurrentAxeNumber = 0;
    public int firstQuestAcquireAxeNumber = 1;
    public int firstQuestCurrentPickAxeNumber = 0;
    public int firstQuestAcquirePickAxeNumber = 1;
    //[SerializeField]
    public int secondQuestCurrentMeatNumber = 0;
    //[SerializeField]
    public int secondQuestAcquireMeatNumber = 2;
    public int finalQuestCurrentStoneNumber = 0;
    public int finalQuestAcquireStoneNumber = 3;
    public int finalQuestCurrentFireWoodNumber=0;
    public int finalQuestAcquireFireWoodNumber=1;

    
    public int currentWoodNumber = 0; //획득한 나무갯수
    public int currentStoneNumber = 0; //획득한 부싯돌갯수

    public bool isSecondEventPlay = false;
    public bool isFinalEventPlay = false;

    public bool isOpenCraftMenu;
    
    private ItemEffectDatabase theItemEffectDatabase;
    

    // Use this for initialization
    void Start ()
    {
        instance = this;
        currentQuestNumber = 0;
        isOpenCraftMenu = false;
        theItemEffectDatabase = FindObjectOfType<ItemEffectDatabase>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        ShowQuestText();
        CheckChangeQuest();
        //건축 임시유아이
        //KeyInputCraftMenu();
        //ShowCraftMenu();
        //Debug.Log(finalQuestCurrentStoneNumber);
    }

    public void KeyInputCraftMenu()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            isOpenCraftMenu = !isOpenCraftMenu;
        }

    }
    public void ShowCraftMenu()
    {
        if (isOpenCraftMenu)
            craftMenu.SetActive(true);
        else if(!isOpenCraftMenu)
            craftMenu.SetActive(false);
    }


    public void CloseCraftMenu()
    {
        isOpenCraftMenu = !isOpenCraftMenu;
    }

    public void ShowGameClearText()
    {
        gameClearText.SetActive(true);
    }

    public void AddCurrentAxeNumber()
    {
        ++firstQuestCurrentAxeNumber;
    }
    public void AddCurrentPickAxeNumber()
    {
        ++firstQuestCurrentPickAxeNumber;
    }
    public void AddCurrentMeatNumber()
    {
        ++secondQuestCurrentMeatNumber;
    }
    public void AddCurrentFireWoodNumber()
    {
        ++finalQuestCurrentFireWoodNumber;
    }
    public void AddCurrentStoneNumber()
    {
        ++finalQuestCurrentStoneNumber;
    }

    public bool IsFirstQuestClear()
    {
        if ((firstQuestCurrentAxeNumber == firstQuestAcquireAxeNumber) && (firstQuestCurrentPickAxeNumber == firstQuestAcquirePickAxeNumber))
        {
            return true;
        }
        else
            return false;
    }

    public bool IsSecondQuestClear()
    {
        if (secondQuestCurrentMeatNumber == secondQuestAcquireMeatNumber)
        {
            return true;
        }
        else
            return false;
    }

    public bool IsFinalQuestClear()
    {
        if ((finalQuestCurrentFireWoodNumber == finalQuestAcquireFireWoodNumber) && (finalQuestCurrentStoneNumber == finalQuestAcquireStoneNumber))
        {
            return true;
        }
        else
            return false;
    }

    private void CheckChangeQuest()
    {
        if (currentQuestNumber == 2)
        {
            if (secondQuestCurrentMeatNumber == secondQuestAcquireMeatNumber)
            {
                if (!isSecondEventPlay)
                {
                    EventManager.instance.eventNumber = 9;
                    isSecondEventPlay = true;
                }
            }
        }
        if (currentQuestNumber == 3)
        {
            if ((finalQuestCurrentFireWoodNumber == finalQuestAcquireFireWoodNumber))
            {
                if (!isFinalEventPlay)
                {
                    EventManager.instance.eventNumber = 11;
                    isFinalEventPlay = true;
                }
            }
        }
    }
   
    public void MakeFireWood()
    {
        CloseCraftMenu();
        //플레이어 앞에 모닥불 생성
        if (finalQuestCurrentStoneNumber >= 3)
        {
            Instantiate(goFireWoodPrefab, new Vector3(Player.transform.position.x + 3.0f, Player.transform.position.y - 2.0f, Player.transform.position.z + 10.0f), Quaternion.Euler(0, 0, 0));
            finalQuestCurrentStoneNumber -= 3;
            finalQuestCurrentFireWoodNumber++;
        }

    }

    private void ShowQuestText()
    {
        switch (currentQuestNumber)
        {
            case 0:
                questNameText.text = "";
                firstQuest.SetActive(false);
                SecondQuest.SetActive(false);
                FinalQuest.SetActive(false);
                break;
            case 1:
                firstQuest.SetActive(true);
                SecondQuest.SetActive(false);
                FinalQuest.SetActive(false);
                questNameText.text = "장비를 얻자!";
                firstQuestTexts[0].text = "도끼 얻기(" + firstQuestCurrentAxeNumber + "/" + firstQuestAcquireAxeNumber + ")";
                firstQuestTexts[1].text = "곡괭이 얻기(" + firstQuestCurrentPickAxeNumber + "/" + firstQuestAcquirePickAxeNumber + ")";
                break;
            case 2:
                firstQuest.SetActive(false);
                SecondQuest.SetActive(true);
                FinalQuest.SetActive(false);
                questNameText.text = "먹을것을 구하자!";
                SecondQuestText.text = "고기 얻기(" + secondQuestCurrentMeatNumber + "/" + secondQuestAcquireMeatNumber + ")";
                break;
            case 3:
                firstQuest.SetActive(false);
                SecondQuest.SetActive(false);
                FinalQuest.SetActive(true);
                questNameText.text = "밤을이겨낼 도구를 만들자!";
                FinalQuestTexts[0].text = "모닥불 제작(" + finalQuestCurrentFireWoodNumber + "/" + finalQuestAcquireFireWoodNumber + ")";
                break;
            default:
                break;
        }
    }
}
