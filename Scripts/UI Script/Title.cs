using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Title : MonoBehaviour {

    public string currentSceneName = "";
    public string mainSceneName = "Graduated_Project";
    public string tutoSceneName = "Tuto";
    public string titleSceneName = "Title_Scene";
    public string endingSceneName = "Ending_Scene";
    public string trueendingSceneName = "Ending_Scene_True";
    public string startcutSceneName= "StartCut_Scene";

    private SaveLoad theSaveLoad;

    // 마우스 포인터로 사용할 텍스쳐 입력 받기
    [SerializeField]
    private Texture2D BasicCursorTexture;
    private Vector2 hotSpot = Vector2.zero;

    [SerializeField]
    private Image sceneLodingSlider;
    [SerializeField]
    private Text sceneLodingText;

    public static bool isTitle = true;

    //동영상
    private VideoPlayer videoPlayer;
    [SerializeField]
    private VideoClip video_Title;
    [SerializeField]
    private VideoClip video_Title2;

    //로딩패널 이미지
    [SerializeField]
    private Sprite[] LoadingImage;

    [SerializeField]
    private GameObject loadingPanel;
    [SerializeField]
    private GameObject textPanel;

    //싱글턴//
    #region 싱글턴
    public static Title instance;
    private void Awake()
    {
        if (instance == null)
        {
            //비디오 플레이어 셋팅//
            videoPlayer = gameObject.GetComponent<VideoPlayer>();

            videoPlayer.clip = video_Title;
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.playOnAwake = true;                             //첫시작시 영상 시작함.
            videoPlayer.waitForFirstFrame = true;                       // 영상이 로드 될때 까지 첫프레임을 기다린다.
            videoPlayer.isLooping = true;                               //첫화면은 타이틀영상이므로 반복재생
            videoPlayer.playbackSpeed = 1.0f;                           //기본배속 재생
            videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;    //카메라에서 farPlane에서 동영상 실행
            //videoPlayer.source = VideoSource.VideoClip;
            //videoPlayer.clip = video_Title;

            Cursor.SetCursor(BasicCursorTexture, hotSpot, CursorMode.Auto);
            Cursor.visible = true;
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this.gameObject);
        //loadingPanel.GetComponent<Image>().sprite
    }
    #endregion
    

    //씬이 다시 로드되었을때 실행됨 //이 함수는 Awake()함수보다 먼저 실행된다. //최초의 씬에서는 실행되지않는다.
    private void OnLevelWasLoaded(int level)
    {
        if(level ==0) //타이틀씬일경우
        {
            videoPlayer = gameObject.GetComponent<VideoPlayer>();
            videoPlayer.targetCamera = Camera.main;
            videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
            textPanel.SetActive(true); //텍스트 패널활성화(타이틀만)

            SoundManager.instance.StopAllSE();
            SoundManager.instance.StopAllBGM();
            SoundManager.instance.PlayBGM("MainBGM");
            gameObject.GetComponent<VideoPlayer>().enabled = true;
            int range = Random.Range(1, 3);
            Text[] titleText = { GameObject.Find("TitleText").GetComponent<Text>(),
                                     GameObject.Find("TutoButtonText").GetComponent<Text>(), GameObject.Find("StartButtonText").GetComponent<Text>(),
                                     GameObject.Find("LoadButtonText").GetComponent<Text>(), GameObject.Find("ExitButtonText").GetComponent<Text>()};
            if (range == 1)
            {
                videoPlayer.clip = video_Title2;
                //타이틀 배경에 맞게 텍스트들의 색깔을 변경

                for (int i = 0; i < titleText.Length; i++)
                {
                    titleText[i].color = Color.white;
                }

            }
            else if (range == 2)
            {
                videoPlayer.clip = video_Title;
            }
            //Cursor.visible = true;
        }
        else if(level ==2) //메인씬
        {
            SoundManager.instance.StopAllSE();
            SoundManager.instance.StopAllBGM();
            SoundManager.instance.PlayBGM("BGM");
            gameObject.GetComponent<VideoPlayer>().enabled = false;

            //theSaveLoad = FindObjectOfType<SaveLoad>();
            //theSaveLoad.LoadData();

            GameOver.isGameOver = false;
            GameOver.isGameClear = false;
            loadingPanel.SetActive(false);
            textPanel.SetActive(false);
        }
        else if(level ==1)
        {
            SoundManager.instance.StopAllSE();
            SoundManager.instance.StopAllBGM();
            SoundManager.instance.PlayBGM("TutoBGM");
            gameObject.GetComponent<VideoPlayer>().enabled = false; //타이틀 씬말고 나머지 씬에서는 동영상 플레이어를 꺼준다.
            GameOver.isGameOver = false;
            GameOver.isGameClear = false;
            loadingPanel.SetActive(false);
            textPanel.SetActive(false);
        }
       else if(level ==3) //엔딩씬(노말 헬기엔딩)
        {
            SoundManager.instance.StopAllSE();
            SoundManager.instance.StopAllBGM();
            SoundManager.instance.PlayBGM("EndingBGM");
            gameObject.GetComponent<VideoPlayer>().enabled = false; //타이틀 씬말고 나머지 씬에서는 동영상 플레이어를 꺼준다.
            loadingPanel.SetActive(false);
            textPanel.SetActive(false);
        }

        else if (level == 4) //첫시작 컷씬
        {
            //VideoPlayer cutScenePlayer = GameObject.Find("CutSceneVideoPlayer").GetComponent<VideoPlayer>();
            //cutScenePlayer.Play();
            SoundManager.instance.StopAllSE();
            SoundManager.instance.StopAllBGM();
            gameObject.GetComponent<VideoPlayer>().enabled = false; //타이틀 씬말고 나머지 씬에서는 동영상 플레이어를 꺼준다.
            loadingPanel.SetActive(false);
            textPanel.SetActive(false);
        }
        else if (level == 5) //엔딩씬(뗏목 트루엔딩)
        {
            SoundManager.instance.StopAllSE();
            SoundManager.instance.StopAllBGM();
            SoundManager.instance.PlayBGM("TrueEndingBGM");
            gameObject.GetComponent<VideoPlayer>().enabled = false; //타이틀 씬말고 나머지 씬에서는 동영상 플레이어를 꺼준다.
            loadingPanel.SetActive(false);
            textPanel.SetActive(false);
        }
    }
    
    
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "Title_Scene")
            Cursor.visible = true;
        CheckTitle();

       // Debug.Log("isTitle:" + isTitle);
       // Debug.Log("Cursor:"+Cursor.visible);
    }

    public void CheckTitle()
    {
        if (SceneManager.GetActiveScene().name == "Title_Scene")
            isTitle = true;
        else
            isTitle = false;
    }

    public void ClickTitle()
    {
        SoundManager.instance.PlaySE("ButtonClick");
        StartCoroutine(TitleStartCoroutine());
    }

    public void ClickTuto()
    {
        SoundManager.instance.PlaySE("ButtonClick");
        StartCoroutine(TutoStartCoroutine());
    }

    public void ClickStart()
    {
        Debug.Log("타이틀 시작 로딩");
        SoundManager.instance.PlaySE("ButtonClick");
        StartCoroutine(GameStartCoroutine());
        //SceneManager.LoadScene(mainSceneName);
    }

    public void ClickLoad()
    {
        Debug.Log("로드");
        SoundManager.instance.PlaySE("ButtonClick");
        //PauseMenu.instance.ClickLoad();
        StartCoroutine(LoadCoroutine());
    }

    public void ClickEnding()
    {
        StartCoroutine(EndingStartCoroutine());
    }

    public void ClickTrueEnding()
    {
        StartCoroutine(TrueEndingStartCoroutine());
    }

    public void StartCutScene()
    {
        StartCoroutine(CutSceneStartCoroutine());
    }

    IEnumerator TutoStartCoroutine()
    {
        RandomLoadingImage();
        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(tutoSceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone) //씬전환이되는 동안 계속대기
        {
            yield return null;
            LoadingProgress(operation);
        }
        loadingPanel.SetActive(false);
        textPanel.SetActive(false);
        //gameObject.SetActive(false);
    }

    public IEnumerator TitleStartCoroutine()
    {
        RandomLoadingImage();
        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(titleSceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone) //씬전환이되는 동안 계속대기
        {
            yield return null;
            LoadingProgress(operation);
        }
        Cursor.visible = true;
        loadingPanel.SetActive(false);
        //gameObject.SetActive(false);
    }

    IEnumerator CutSceneStartCoroutine()
    {
        RandomLoadingImage();
        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(startcutSceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone) //씬전환이되는 동안 계속대기
        {
            yield return null;
            LoadingProgress(operation);
        }
        Cursor.visible = true;
        loadingPanel.SetActive(false);
        textPanel.SetActive(false);
    }

    IEnumerator EndingStartCoroutine()
    {
        RandomLoadingImage();
        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(endingSceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone) //씬전환이되는 동안 계속대기
        {
            yield return null;
            LoadingProgress(operation);
        }
        Cursor.visible = true;
        loadingPanel.SetActive(false);
        textPanel.SetActive(false);
        //gameObject.SetActive(false);
    }

    IEnumerator TrueEndingStartCoroutine()
    {
        RandomLoadingImage();
        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(trueendingSceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone) //씬전환이되는 동안 계속대기
        {
            yield return null;
            LoadingProgress(operation);
        }
        Cursor.visible = true;
        loadingPanel.SetActive(false);
        textPanel.SetActive(false);
        //gameObject.SetActive(false);
    }

    IEnumerator GameStartCoroutine()
    {
        RandomLoadingImage();
        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(mainSceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone) //씬전환이되는 동안 계속대기
        {
            yield return null;
            LoadingProgress(operation);
        }
        loadingPanel.SetActive(false);
        textPanel.SetActive(false);
        //gameObject.SetActive(false);
    }

    IEnumerator LoadCoroutine()
    {
        RandomLoadingImage();
        //씬정보가 로드전 필요하기때문에 약간의 딜레이를 주고 그사이 씬데이터를 로드한다.(하드코딩)
        //theSaveLoad = FindObjectOfType<SaveLoad>();
        //theSaveLoad.LoadSceneData();
        //yield return new WaitForSeconds(0.7f);

        loadingPanel.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(mainSceneName);
        operation.allowSceneActivation = false;
        while (!operation.isDone) //씬전환이되는 동안 계속대기
        {
            yield return null;
            sceneLodingSlider.fillAmount = operation.progress;
            sceneLodingText.text = "로딩중 입니다......" + Mathf.Floor(operation.progress * 100.0f) + "%"; //진행도를 소수점 첫째자리내림으로 표현
            if (operation.progress == 0.9f)
            {
                sceneLodingSlider.fillAmount = 1.0f;
                sceneLodingText.text = "로딩 완료!!!! " + 100 + "%";
                operation.allowSceneActivation = true; //로딩 진행도가 0.9 가되었을시 비동기 scene로딩 처리
            }
            LoadingProgress(operation);
        }
        Debug.Log("로드코루틴 종료");

        //다끝나고 로드
        theSaveLoad = FindObjectOfType<SaveLoad>();
        theSaveLoad.LoadData();

        loadingPanel.SetActive(false); //로딩화면
        textPanel.SetActive(false);
        //gameObject.SetActive(false); //파괴되지 않게 dontDestroy로 설정했기때문에 setactive비활성화시킴.
        //Destroy(gameObject);
    }

    public void ClickExit()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }

    private void RandomLoadingImage()
    {
        int range = Random.Range(0, LoadingImage.Length);
        switch (range)
        {
            case 0:
                loadingPanel.GetComponent<Image>().sprite = LoadingImage[0];
                break;
            case 1:
                loadingPanel.GetComponent<Image>().sprite = LoadingImage[1];
                break;
            case 2:
                loadingPanel.GetComponent<Image>().sprite = LoadingImage[2];
                loadingPanel.GetComponentInChildren<Text>().color = Color.white;
                break;
        }
    }

    private void LoadingProgress(AsyncOperation _operation)
    {

        //로딩 진행도를 슬라이더 UI값과 텍스트에 저장
        sceneLodingSlider.fillAmount = _operation.progress;
        sceneLodingText.text = "로딩중 입니다......" + Mathf.Floor(_operation.progress * 100.0f) + "%"; //진행도를 소수점 첫째자리내림으로 표현
        if (_operation.progress == 0.9f)
        {
            sceneLodingSlider.fillAmount = 1.0f;
            sceneLodingText.text = "로딩 완료!!!! " + 100 + "%";
            _operation.allowSceneActivation = true; //로딩 진행도가 0.9 가되었을시 비동기 scene로딩 처리
        }
    }
}
