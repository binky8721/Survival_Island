using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayAndNight : MonoBehaviour {

    [SerializeField]
    private GameObject clockHand; //낮밤UI시계
    private RectTransform clockRectTransform;

    [SerializeField]
    public static float secondPerRealTimeSecond = 5; //게임 세계의 5초 = 현실 세계의 1초

    public static bool isNight = false;

    [SerializeField]
    private float fogDensityCalc; //Fog밀도 증감량비율

    [SerializeField]
    private float nightFogDensity; // 밤 상태의 Fog밀도 0.07
    private float dayFogDensity; //낮 상태의 Fog밀도 0.0002
    private float currentFogDensity;

    private bool fogEnd = false;

    [SerializeField]
    private Material daySky;

	// Use this for initialization
	void Start () {
        dayFogDensity = RenderSettings.fogDensity; //시작 Fog밀도는 현재 랜더세팅 Fog밀도로 초기화 (0.02)
        clockRectTransform = clockHand.GetComponent<RectTransform>();
	}

    public void SetMorning()
    {
        transform.rotation = Quaternion.Euler(50.0f, -30.0f, 0.0f);
        clockRectTransform.rotation = Quaternion.Euler(0, 0, -50f);

    }

    public void SetNight()
    {
        transform.rotation = Quaternion.Euler(180, -30.0f, 0.0f);
        clockRectTransform.rotation = Quaternion.Euler(0, 0, -180f);
    }

    public void SetSecondPerRealTime(float time)
    {
        secondPerRealTimeSecond = time;
    }

    private void ChangeCheatRealTime()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            secondPerRealTimeSecond += 25;
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            secondPerRealTimeSecond -= 25;
    }
	
	// Update is called once per frame
	void Update () {
        ChangeCheatRealTime();
        if (!GameManager.isPause) //게임이 일시정지가 아닐때에만 시간이 흐른다.
        {
            transform.Rotate(Vector3.right, 0.1f * secondPerRealTimeSecond * Time.deltaTime);

            // 태양의 각도가 170도이상이면 밤 340도(-10도와 같은값)이하면 낮
            if (transform.eulerAngles.x >= 170)
                isNight = true;
            else if (transform.eulerAngles.x <= 340)
                isNight = false;

            //밤낮 UI 시계추 회전
            clockRectTransform.Rotate(new Vector3(0, 0, -0.1f * secondPerRealTimeSecond * Time.deltaTime));
            
            //스카이박스 회전
            RenderSettings.skybox.SetFloat("_Rotation", Time.time*0.5f);
            //RenderSettings.skybox.SetFloat("_Exposure", Mathf.Sin(Time.time * Mathf.Deg2Rad * secondPerRealTimeSecond) + 1);

            if (isNight) //밤일경우 밀도를 밤밀도가 될때까지 점점 늘린다.
            {
                secondPerRealTimeSecond = 30;
                //fogEnd = false;
                //RenderSettings.fog = true;
                if (currentFogDensity <= nightFogDensity)
                {
                    currentFogDensity += 0.1f * fogDensityCalc * Time.deltaTime;
                    RenderSettings.fogDensity = currentFogDensity;
                }

                //횃불같은 광원을 플레이어가 들었을시 순간적으로 밀도를 낮추는 코드 추가예정
                //ex
                /*
                 if(횃불)
                 {
                    currentFogDensity += 0.5f; //(임의의값)
                    RenderSettings.fogDensity = currentFogDensity;
                 }
                */
            }
            else //낮일경우 밀도를 낮밀도가 될때까지 점점 낮춘다.
            {
                secondPerRealTimeSecond = 2.5f;
                //낮 스카이박스로 교체
                RenderSettings.skybox = daySky;
                RenderSettings.sun = gameObject.GetComponent<Light>();
                //if (!fogEnd)
                //{
                //    fogEnd = true;
                //    StartCoroutine(FogEnd());
                //}
                if (currentFogDensity >= dayFogDensity)
                {
                    currentFogDensity -= 0.1f * fogDensityCalc * Time.deltaTime;
                    RenderSettings.fogDensity = currentFogDensity;
                }
            }
        }
	}

    /*
    IEnumerator FogEnd()
    {
        yield return new WaitForSeconds(10.0f);

        RenderSettings.fog = false;
    }
    */

}
