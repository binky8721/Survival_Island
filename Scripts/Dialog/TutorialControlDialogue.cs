using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class TutorialControlDialogue : MonoBehaviour {

    public static bool dialogActivated = false;

    //필요한 컴포넌트
    [SerializeField]
    private Text nameText; //이름
    [SerializeField]
    private Text dialogueText; //대사
    [SerializeField]
    private Image standImage;
    [SerializeField]
    private Image panelImage;
    [SerializeField]
    private GameObject eventTextboxParent;

    TutorialDialogue theTutorialEventManager;



    private string nameString;
    private string scriptString;
    private string imageString;

    //텍스트 애니메이션 변수
    private string animateText; //텍스트 애니메이션 문자열
    public int cntForAnimate;

    private bool isActive; //대화 실행여부
    private bool isScriptEnd;//현재의 대사가 끝났는지의 여부

    public JsonData convertedData; //Json의 객체로, 캐릭터의 이름, 대사, 이미지의 상태 정보

    //대화 관련 변수
    private int currentIndex;
    private int endOfLine; //대사의 끝

    private bool isEventPlus;

    void Start()
    {
        theTutorialEventManager = FindObjectOfType<TutorialDialogue>();
        currentIndex = 0;
        cntForAnimate = 0;
        isEventPlus = false;
    }


    void Update()
    {
        TextSkipAnimation();
    }

    private void TextSkipAnimation()
    {
        if (isActive)
        {
            if (isScriptEnd == false)            //대사가 진행중인 상황에서
            {
                if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))         //r키를 누르거나 마우스왼쪽클릭시 스킵
                {
                    StopTextAnime();
                }

            }
            else                                 //대사가 끝난 상태에서
            {
                if (Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0))        //r키를 누르거나 마우스왼쪽클릭시
                {
                    //Debug.Log("Next");
                    ChangeSettings();
                }
            }

        }
        else
            return;
    }

    private void StopTextAnime()
    {
        StopAllCoroutines();
        dialogueText.text = scriptString;          //모든 대사가 한번에 표시
        isScriptEnd = true;
        cntForAnimate = 0;
    }

    public void LoadJSON(JsonData _convertedData) //객체와 상호작용할때마다 호출되는 함수 (해당 객체의 JSON을 로드함)  **이벤트넘버올릴때**
    {
        dialogActivated = !dialogActivated;

        currentIndex = 0;

        isActive = true;

        isScriptEnd = false;

        convertedData = _convertedData;

        animateText = "";
        dialogueText.text = "";

        endOfLine = convertedData["dialogues"].Count;
        //Empty.SetActive(true);

        isEventPlus = true;

        ChangeSettings();

        eventTextboxParent.gameObject.SetActive(true);
        //isSpecial = false;
    }

    void ChangeSettings()
    {
        if (currentIndex == endOfLine)
        {
            currentIndex = 0;
            isActive = false;
            dialogActivated = !dialogActivated;
            theTutorialEventManager.doingEvent = false;
            //Empty.SetActive(false);
            if (isEventPlus)
                theTutorialEventManager.tu_eventNumber++;
            eventTextboxParent.gameObject.SetActive(false);
            animateText = "";
            dialogueText.text = "";
            nameText.text = "";
            currentIndex = 0;
            cntForAnimate = 0;
            return;
        }
        scriptString = convertedData["dialogues"][currentIndex]["script_Text"].ToString();
        nameText.text = convertedData["dialogues"][currentIndex]["character_name"].ToString();
        imageString = convertedData["dialogues"][currentIndex]["standImage_Name"].ToString();

        standImage.sprite = Resources.Load<Sprite>("StandImage/" + imageString);

        isScriptEnd = false;

        StartCoroutine(TextAnimation(1f));
        currentIndex++; //다음대사로

    }

    IEnumerator TextAnimation(float stringspeed)
    {
        if (!isScriptEnd)
        {
            dialogueText.text = "";
            while (dialogueText.text != scriptString)
            {
                dialogueText.text += scriptString[cntForAnimate];
                cntForAnimate++;

                yield return new WaitForSeconds(stringspeed * Time.deltaTime);
            }
            cntForAnimate = 0;
            isScriptEnd = true;
            yield break;
        }
        else
        {
            //Debug.Log("도중뻥");
            yield break;
        }
    }
}
