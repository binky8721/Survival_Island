using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class EventManager : MonoBehaviour {

    public GameObject Respawn;

    //자기 자신 반환 인스턴스 (다른곳에서 자주쓰이므로 스테틱으로 썼음)
    public static EventManager instance;

    public bool doingEvent;
    public int eventNumber =1;
    public int tempEventNumber = 2;
    public bool firstEventSee = false;

    private ControlDialogue theControlDialog;
    private TextAsset textData;
    private JsonData jsonData;

    //하드코딩
    [SerializeField]
    private GameObject goBaseUi;
    [SerializeField]
    private GameObject CraftMenu;
    [SerializeField]
    private GameObject wayPoint;
    [SerializeField]
    private GameObject wayPointArrow;
    [SerializeField]
    private GameObject cheatItem;

    private FadeInOut theFadeInOut;
    private DayAndNight theDayAndNight;

    public bool isFireWoodEventClear = false; //첫번째 모닥불퀘스트 변수
    public bool isFirstNightClear = false; //첫번째 밤 퀘스트 견뎠는지 여부 변수

    void Start ()
    {
        instance = this;
        theControlDialog = FindObjectOfType<ControlDialogue>();
        doingEvent = false;
        if (eventNumber == 0)
        {
            eventNumber = 0;
            tempEventNumber = 1;
        }
        theFadeInOut = FindObjectOfType<FadeInOut>();
        theDayAndNight = FindObjectOfType<DayAndNight>();
        StartCoroutine(StartEvent());
    }
	
	void Update ()
    {
        if (TutorialDialogue.instance == null) //튜토리얼이 꺼져있을때 활성화
        {
            EventList();
        }
        CheckCheatCode();
        //CheckFirstStageGameover();
    }

    IEnumerator StartEvent()
    {
        yield return new WaitForSeconds(0.5f);
        if(!firstEventSee) //첫 게임시작시 (노세이브데이터)
        {
            PlayEvent("FirstEvent");
            QuestUI.instance.currentQuestNumber = 1;
            firstEventSee = true;
        }
        else // 게임 불러오기로 시작시
        {
            QuestUI.instance.currentQuestNumber = QuestUI.instance.savedQuestNumber;
        }
    }

    //메인씬내 치트코드
    public void CheckCheatCode()
    {
        //test용(모닥불클리어후 밤 이벤트로 이동)
        if (Input.GetKeyDown(KeyCode.G))
        {
            QuestUI.instance.currentQuestNumber = 0; //퀘스트 아무것도 없음
            eventNumber = 12;
        }
        //test용(밤을 견딘후 이벤트로 이동)
        if (Input.GetKeyDown(KeyCode.H))
        {
            FindObjectOfType<NPCRespawn>().DestroyMonster();
            theDayAndNight.SetMorning();
            QuestUI.instance.currentQuestNumber = 0; //퀘스트 아무것도 없음
            eventNumber = 15;
        }
        //여러 재료 아이템 얻기
        if (Input.GetKeyDown(KeyCode.T))
        {
            cheatItem.SetActive(true);
        }

        //test용 (게임오버 치트코드)
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            isGameOver = true;
        }
        Debug.Log(isGameOver);
        */
    }

    public void HungryEvent()
    {
        textData = Resources.Load<TextAsset>("EventDialogue/FirstEvent");
        jsonData = JsonMapper.ToObject(textData.text);
        theControlDialog.LoadJSON(jsonData);
    }

    public void PlayEvent(string eventName)
    {
        doingEvent = true;
        textData = Resources.Load<TextAsset>("EventDialogue/"+eventName);
        jsonData = JsonMapper.ToObject(textData.text);
        theControlDialog.LoadJSON(jsonData);
    }
    public void GameClear()
    {
        Title.instance.ClickEnding();
    }

    public void GameClearTrue()
    {
        Title.instance.ClickTrueEnding();
    }
    /*
    public void CheckFirstStageGameover()
    {
        //1스테이지이벤트 진행도중 밤이되었을시 게임오버
        if((eventNumber>=1 && eventNumber<=10) && DayAndNight.isNight)
        {
            GameOver.reasonText.text
            GameOver.isGameOver = true;
        }
    }
    */

    public void EventList()
    {
        /* //이벤트넘버 체크용 버튼코드
        if (Input.GetKeyDown(KeyCode.K))
        {
            eventNumber++;
            Debug.Log(eventNumber);
        }
        */

        if (tempEventNumber == eventNumber)
        {
            return;
        }
        else
        {
            switch (eventNumber)
            {
                //맨처음 게임시작 대사
                case 0:
                    //PlayEvent("FirstEvent");
                    //firstEventSee = true;
                    //tempEventNumber = 1;
                    break;
                case 1:
                    wayPoint.SetActive(false); //하드코딩: 웨이포인트 처음대사끝나고 꺼놓기
                    doingEvent = false;
                    //QuestUI.instance.currentQuestNumber = 1;
                    break;
                //맨처음 도끼 얻었을시
                case 3:
                    PlayEvent("GetAxe");
                    break;
                case 4:
                    doingEvent = false;
                    if (QuestUI.instance.IsFirstQuestClear())
                    {
                        PlayEvent("FirstQuestClear");
                        eventNumber = 6;
                    }
                    break;
                //맨처음 곡괭이 얻었을시
                case 5:
                    PlayEvent("GetPickAxe");
                    break;
                case 6: //하드코딩 이벤트 (첫번째퀘스트 완료)
                    doingEvent = false;
                    if (QuestUI.instance.IsFirstQuestClear())
                    {
                        PlayEvent("FirstQuestClear");
                    }
                    break;
                case 7:
                    doingEvent = false;
                    QuestUI.instance.currentQuestNumber = 2;
                    break;
                case 9: //하드코딩 이벤트(두번째퀘스트 완료)
                    PlayEvent("SecondQuestClear");
                    break;
                case 10:
                    doingEvent = false;
                    QuestUI.instance.currentQuestNumber = 3;
                    break;
                case 11: //하드코딩 이벤트(마지막퀘스트 : 모닥불완성하기 완료)
                    QuestUI.instance.currentQuestNumber = 0;
                    PlayEvent("FinalQuestClear");
                    break;
                case 12: //게임 클리어 화면 등장
                    //GameManager.isPause = true; //화면 일시정지
                    //goBaseUi.SetActive(true);
                    isFireWoodEventClear = true;
                    doingEvent = false;
                    CraftMenu.SetActive(false);
                    theFadeInOut.isFadeIn = true; //페이드인아웃 실행
                    theFadeInOut.isEventdoing = true;
                    break;
                case 13:
                    SoundManager.instance.StopAllBGM();
                    theDayAndNight.SetNight();
                    //theDayAndNight.SetMorning();
                    theFadeInOut.isFadeOut = true;
                    PlayEvent("SecondEventStart");

                    break;
                case 14:
                    doingEvent = false;
                    Respawn.SetActive(true);
                    SoundManager.instance.PlayBGM("PinchBGM");
                    //Respawn.GetComponent<NPCRespawn>().isSpawn = true;
                    break;
                
                //아침이 된후 이벤트(15~16)
                case 15:
                    if (!GameOver.isGameOver)
                    {
                        theDayAndNight.SetMorning();
                        PlayEvent("SecondEventStartClear");
                        //Respawn.GetComponent<NPCRespawn>().isSpawn = false;
                        Respawn.SetActive(false);
                        SoundManager.instance.StopAllBGM();
                        DayAndNight.secondPerRealTimeSecond = 5;
                        isFirstNightClear = true;
                    }
                    break;
                case 16:
                    SoundManager.instance.PlayBGM("MainBGM");
                    doingEvent = false;
                    break;

                //구조신호 완성후 이벤트(17-18)
                case 17:
                    SoundManager.instance.StopAllBGM();
                    SoundManager.instance.PlaySE("Helicopter");
                    PlayEvent("SecondEventStartClear2");
                    break;
                case 18:
                    SoundManager.instance.PlayBGM("MainBGM");
                    doingEvent = false;
                    SoundManager.instance.StopSE("Helicopter");
                    wayPointArrow.SetActive(true);
                    wayPoint.SetActive(true);
                    break;
                
                //땟목 완성 이벤트(트루엔딩)
                case 19:
                    SoundManager.instance.StopAllBGM();
                    PlayEvent("TrueEnding");
                    break;
                case 20:
                    doingEvent = false;
                    if(!CraftManual.isMakeRaft)
                    {
                        GameClearTrue();
                        CraftManual.isMakeRaft = true;
                    }
                    break;

                //게임클리어
                /*
                case 17:
                    doingEvent = false;
                    if (!GameOver.isGameClear)
                        GameOver.isGameClear = true;
                    break;
                    */

                    //맨처음 서브머신건 1 얻었을시
                    /*
                    case 7:
                        textData = Resources.Load<TextAsset>("EventDialogue/GetSubMachineGun1");
                        jsonData = JsonMapper.ToObject(textData.text);
                        theControlDialog.LoadJSON(jsonData);
                        break;
                    */
            }
            tempEventNumber = eventNumber;
        }
    }
}
