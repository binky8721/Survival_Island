
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class TutorialDialogue : MonoBehaviour {

    public static TutorialDialogue instance;
    public bool isTutorial;

    public bool doingEvent;
    public int tu_eventNumber;
    public int tu_tempEventNumber;

    private TutorialControlDialogue theControlDialog;
    private TextAsset textData;
    private JsonData jsonData;

    //하드코딩
    [SerializeField]
    private GameObject goBaseUi;
    [SerializeField]
    private GameObject CraftMenu;
    [SerializeField]
    private GameObject FirstDestination;
    [SerializeField]
    private GameObject tutorialAxe;
    [SerializeField]
    private GameObject tutorialAxeHolder;
    [SerializeField]
    private GameObject tutorialFrame;
    [SerializeField]
    private GameObject tutorialArrow;



    private bool isGetAxe_tu;
    private bool isEndTutorial = false;

    private FadeInOut theFadeInOut;
    private DayAndNight theDayAndNight;

   
    void Start ()
    {
        instance = this;
        isTutorial = true;

        theControlDialog = FindObjectOfType<TutorialControlDialogue>();
        doingEvent = false;
        tu_eventNumber = 1;
        tu_tempEventNumber = 2;

        theFadeInOut = FindObjectOfType<FadeInOut>();
        theDayAndNight = FindObjectOfType<DayAndNight>();

        isGetAxe_tu = false;
        tutorialAxe.SetActive(false);
    }

    void Update()
    {
        if (isTutorial)
            TutorialEventList();
        if (!isGetAxe_tu)
            SecondTutorialCheck();
        ThirdTutorialCheck();
    }

    public void SecondTutorialCheck()
    {
        if (tutorialAxe==null)
        {
            isGetAxe_tu = true;
            tu_eventNumber = 5; //두번째 튜토리얼완료
        }
    }

    public void SecondTutorialEvent()
    {
        tutorialAxe.SetActive(true);
    }

    public void ThirdTutorialCheck()
    {
        if (tutorialAxeHolder.activeSelf && tu_eventNumber == 6)
            tu_eventNumber = 7;
    }
    

    

    public void TutorialEventList()
    {
        if (tu_tempEventNumber == tu_eventNumber)
        {
            return;
        }
        else
        {
            switch (tu_eventNumber)
            {
                //맨처음 튜토리얼 시작
                case 1:
                    textData = Resources.Load<TextAsset>("TutorialDialogue/FirstTutorial");
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    doingEvent = true;
                    break;
                case 2:
                    doingEvent = false;
                    break;
                //첫번째 튜토 완료 대사
                case 3:
                    textData = Resources.Load<TextAsset>("TutorialDialogue/SecondTutorial");
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    SecondTutorialEvent();
                    doingEvent = true;
                    break;
                case 4:
                    doingEvent = false;
                    break;
                case 5: //두번째 튜토 완료
                    doingEvent = true;
                    textData = Resources.Load<TextAsset>("TutorialDialogue/ThirdTutorial");
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    break;
                case 6:
                    doingEvent = false;
                    break;
                case 7: //세번째 튜토 완료
                    goBaseUi.SetActive(false);
                    Inventory.inventoryActivated = !Inventory.inventoryActivated;
                    doingEvent = true;
                    textData = Resources.Load<TextAsset>("TutorialDialogue/ClearThirdTutorial");
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    break;
                case 8:
                    tutorialFrame.SetActive(true);
                    textData = Resources.Load<TextAsset>("TutorialDialogue/FourTutorial_Start"); //status설명시작
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    break;
                case 9:
                    tutorialFrame.SetActive(false);
                    tutorialArrow.SetActive(true);
                    textData = Resources.Load<TextAsset>("TutorialDialogue/FourTutorial_Hungry"); //hungry설명
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    break;
                case 10:
                    tutorialArrow.GetComponent<RectTransform>().localPosition = new Vector3(-392.5f, 450.0f, 0);
                    textData = Resources.Load<TextAsset>("TutorialDialogue/FourTutorial_Thirsty"); //thirsty설명
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    break;
                case 11:
                    tutorialArrow.GetComponent<RectTransform>().localPosition = new Vector3(-392.5f, 410.0f, 0);
                    textData = Resources.Load<TextAsset>("TutorialDialogue/FourTutorial_HP"); //hp 설명
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    break;
                case 12:
                    tutorialArrow.GetComponent<RectTransform>().localPosition = new Vector3(-392.5f, 375.0f, 0);
                    textData = Resources.Load<TextAsset>("TutorialDialogue/FourTutorial_SP"); //hp 설명
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    break;
                case 13: //낮밤 튜토리얼
                    tutorialArrow.GetComponent<RectTransform>().rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    tutorialArrow.GetComponent<RectTransform>().localPosition = new Vector3(680.0f, 450.0f, 0);

                    theDayAndNight.SetSecondPerRealTime(50);
                    textData = Resources.Load<TextAsset>("TutorialDialogue/FiveTutorial_DayNight");
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    break;
                case 14: //미니맵 튜토리얼
                    tutorialArrow.GetComponent<RectTransform>().localPosition = new Vector3(470.0f, 180.0f, 0);
                    textData = Resources.Load<TextAsset>("TutorialDialogue/SixTutorial_Minimap");
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    break;
                case 15:
                    tutorialArrow.SetActive(false);
                    textData = Resources.Load<TextAsset>("TutorialDialogue/TutorialEnd");
                    jsonData = JsonMapper.ToObject(textData.text);
                    theControlDialog.LoadJSON(jsonData);
                    break;
                case 16: //튜토리얼 종료
                    doingEvent = false;

                    //타이틀로 이동
                    if(!isEndTutorial)
                    {
                        isEndTutorial = true;
                        GameObject.Find("PauseMenu").GetComponent<PauseMenu>().CloseMenu();
                        Title.instance.ClickTitle();
                    }
                    
                    //StartCoroutine(Title.instance.TitleStartCoroutine());
                    //tu_tempEventNumber++;
                    break;
                default:
                    doingEvent = false;
                    break;
            }
            tu_tempEventNumber = tu_eventNumber;
        }
    }
    

}
