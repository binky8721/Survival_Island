using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOver : MonoBehaviour {

    public static bool isGameOver = false;
    public static bool isGameClear = false;

    [SerializeField]
    private GameObject GameOverUI;
    [SerializeField]
    private GameObject GameClearUI;

    [SerializeField]
    private GameObject MovingScene;

    private Animator GameOverAnimator;

    [SerializeField]
    public Text reasonText;

    void Start () {
        GameOverAnimator = GetComponent<Animator>();
        //reasonText = GameObject.Find("ReasonText").GetComponent<Text>();
    }
    
    void CheckGameOver()
    {
        if (isGameOver)
        {
            //Destroy(GameObject.Find("Player"));
            //PauseMenu.instance.CallMenu(); //임시로 게임을 일시정지
            SoundManager.instance.StopAllBGM();
            GameOverUI.SetActive(true);
            GameOverAnimator.SetBool("isGameOver", true);
            //MovingScene.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                isGameOver = false;
                GameOverAnimator.SetBool("isGameOver", false);
                GameOverUI.SetActive(false);
                Title.instance.ClickTitle(); //타이틀 씬으로 이동
            }
        }
        if (!isGameOver)
            GameOverUI.SetActive(false);

        CheckFirstStageGameover();
    }

    void CheckGameClear()
    {
        if (isGameClear)
        {
            GameClearUI.SetActive(true);
            if (Input.GetKeyDown(KeyCode.R))
            {
                isGameClear = false;
                GameClearUI.SetActive(false);
                Title.instance.ClickTitle(); //타이틀 씬으로 이동
            }
        }
        else
            GameClearUI.SetActive(false);
    }

    public void CheckFirstStageGameover()
    {
        //1스테이지이벤트 진행도중 밤이되었을시 게임오버

        /*
        if ((EventManager.instance.eventNumber >= 1 && EventManager.instance.eventNumber <= 10) && DayAndNight.isNight && QuestUI.instance.currentQuestNumber != 0)
        {
            reasonText.text = "(밤이 되었지만 모닥불을 완성하지 못했습니다..)";
            isGameOver = true;
        }
        */
        if (!EventManager.instance.isFireWoodEventClear && DayAndNight.isNight && QuestUI.instance.currentQuestNumber != 0)
        {
            reasonText.text = "(밤이 되었지만 모닥불을 완성하지 못했습니다..)";
            isGameOver = true;
        }
    }

    void Update ()
    {
        CheckGameOver();
        CheckGameClear();
    }
}
