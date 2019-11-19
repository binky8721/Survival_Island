using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    [SerializeField]
    private GameObject goBaseUi;
    //[SerializeField]
    public SaveLoad theSaveLoad;

    public static PauseMenu instance;

    private Title theTitle;

    // Use this for initialization
    void Start () {
        theSaveLoad = FindObjectOfType<SaveLoad>();
        theTitle = FindObjectOfType<Title>();
    }

    // Update is called once per frame
    void Update() {

        if (EventManager.instance != null)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (GameManager.isPause == false)
                    CallMenu();
                else
                    CloseMenu();
            }
        }
        else if (TutorialDialogue.instance != null)//튜토리얼이 켜져있다면
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (TutorialGameManager.t_isPause == false)
                    CallMenu();
                else
                    CloseMenu();
            }
        }

    }

    public void CallMenu()
    {
        if(EventManager.instance!=null)
        {
            GameManager.isPause = true;
            goBaseUi.SetActive(true);
            //Time.timeScale = 0f; 
        }
        else if(TutorialDialogue.instance!=null)//튜토리얼이 켜져있다면
        {
            TutorialGameManager.t_isPause = true;
            goBaseUi.SetActive(true);
           // Time.timeScale = 0f; 
        }

    }

    public void CloseMenu()
    {
        if (EventManager.instance != null)
        {
            //GameManager.isPause = !GameManager.isPause;
            GameManager.isPause = false;
            goBaseUi.SetActive(false);
            //Time.timeScale = 1f; //정상속도 진행
        }
        else if (TutorialDialogue.instance != null)//튜토리얼이 켜져있다면
        {
            //TutorialGameManager.t_isPause = !TutorialGameManager.t_isPause;
            TutorialGameManager.t_isPause = false;
            goBaseUi.SetActive(false);
           // Time.timeScale = 1f; //정상속도 진행
        }
    }

    public void ClickTitle()
    {
        theTitle.ClickTitle();
    }

    public void ClickSave()
    {
        Debug.Log("세이브");
        SoundManager.instance.PlaySE("SaveLoadClick");
        theSaveLoad.SaveData();

    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        SoundManager.instance.PlaySE("SaveLoadClick");
        theSaveLoad.LoadData(); //씬 다시로드말고 데이터만 로딩하고 싶을시
    }

    public void ClickExit()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }
}
