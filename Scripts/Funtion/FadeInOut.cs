using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour {

    //FadeIn이 시작되고 끝난후 FadeOut이 작동되도록 설정함.
    //사용법 : 페이드인아웃을 원하는 타이밍에 isFadeIn을 true로 설정.


    public Image fadeImage;
    float fades = 1.0f;
    float time = 0;

    public bool isFadeIn = false;
    public bool isFadeOut = true;
    public bool isEventdoing = false;

    private GameObject fadeImageObject;

    private void Start()
    {

    }

    void ActivateFadeOut()
    {
        fadeImage.enabled = true;
        time += Time.deltaTime;
        if (fades > 0.0f && time >= 0.1f)
        {
            //마우스왼클릭시 페이드아웃 빠르게진행
            if (Input.GetMouseButtonDown(0))
                fades -= 0.3f;

            fades -= 0.05f;
            fadeImage.color = new Color(0, 0, 0, fades);
            time = 0;
        }
        else if (fades <= 0.0f)
        {
            //다음씬으로 넘어가거나 다음 행동할것들..
            time = 0;
            isFadeOut = false;
            fades = 0.0f;
        }
    }

    void ActivateFadeIn()
    {
        fadeImage.enabled = true;
        time += Time.deltaTime;
        if (fades < 1.0f && time >= 0.1f)
        {
            if (Input.GetMouseButtonDown(0))
                fades += 0.3f;
            fades += 0.05f;
            fadeImage.color = new Color(0, 0, 0, fades);
            time = 0;
        }
        else if (fades >= 1.0f)
        {
            //다음씬으로 넘어가거나 다음 행동할것들..
            time = 0;
            isFadeIn = false;
            //isFadeOut = true;
            fades = 1.0f;
            if(isEventdoing)
             EventManager.instance.eventNumber++;
            isEventdoing = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(isFadeOut)
            ActivateFadeOut();
        else
        {
            fadeImage.enabled = false;
        }
        if(isFadeIn)
            ActivateFadeIn();
        else
        {
            fadeImage.enabled = false;
        }


        //페이드인아웃 테스트용
        //if (Input.GetKeyDown(KeyCode.Numlock))
        //    isFadeIn = true;
        
    }
}
