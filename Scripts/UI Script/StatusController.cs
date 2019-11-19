using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusController : MonoBehaviour {

    
    [SerializeField]
    private int hp; //체력 최대치
    private int currentHp; //현재 체력

    private int HpDecreaseTime = 2; //체력 감소되는 속도(배고픔,목마름시) //2초마다 한번 감소
    private int currentHpDecreaseTime; 

    [SerializeField]
    private int sp; //스테미나 최대치
    private int currentSp; //현재 스테미나

    [SerializeField]
    private int spIncreaseSpeed; //스테미나 회복속도

    [SerializeField]
    private int spRechargeTime; //스테미나 재회복 딜레이.
    private int currentSpRechargeTime;

    // 스태미나 감소 여부.
    private bool spUsed;

    // 방어력
    [SerializeField]
    private int dp;
    private int currentDp;

    // 배고픔
    [SerializeField]
    private int hungry;
    private int currentHungry;

    // 배고품이 줄어드는 속도
    [SerializeField]
    private int hungryDecreaseTime; //만약 이값이 100이면 100초마다 배고픔게이지 일정량감소
    private int currentHungryDecreaseTime;

    // 목마름
    [SerializeField]
    private int thirsty;
    private int currentThirsty;

    // 목마름이 줄어드는 속도
    [SerializeField]
    private int thirstyDecreaseTime;
    private int currentThirstyDecreaseTime;

    // 만족도
    [SerializeField]
    private int satisfy;
    private int currentSatisfy;

    // 필요한 이미지
    [SerializeField]
    private Image[] imagesGauge;

    //각 게이지관련 변수 상수화.
    private const int HP = 0, DP = 1, SP = 2, HUNGRY = 3, THIRSTY = 4, SATISFY = 5;

    //배고픔관련 이벤트(1회성)
    private bool isHungryFirst = false;

    //게임오버관련 변수
    private GameOver theGameOver;

    //데미지받을시 이펙트
    public Image bloodScreen;

    public IEnumerator ShowBloodScreen()
    {
        //Debug.Log("피격 데미지 스크린 코루틴 진행");
        bloodScreen.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.2f, 0.3f));
        yield return new WaitForSeconds(0.3f); //잠시동안 이미지를 보이게한 후
        bloodScreen.color = Color.clear; // 다시 투명화
    }


    void Start ()
    {
        currentHp = hp;
        currentDp = dp;
        currentSp = sp;
        currentHungry = hungry;
        currentThirsty = thirsty;
        currentSatisfy = satisfy;
        theGameOver = FindObjectOfType<GameOver>();
	}
	
	void Update ()
    {
        if (EventManager.instance != null) //이벤트매니져가 켜져있다면 (통상스테이지 플레이시)
        {
            if (!EventManager.instance.doingEvent && !GameManager.isPause) //하드코딩(이벤트 대사중일때는 게이지가 닳지 않는다.) + 일시정지일때도 게이지가 닳지 않는다.
            {
                Hungry();
                Thirsty();
                ConstantDecreaseHP();
                GaugeUpdate();
                SPRechargeTime();
                SPRecover();
            }
        }
        else if(TutorialDialogue.instance !=null) //튜토리얼이 켜져있을시
        {
            if (!TutorialDialogue.instance.doingEvent && !TutorialGameManager.t_isPause) //하드코딩(이벤트 대사중일때는 게이지가 닳지 않는다.) + 일시정지일때도 게이지가 닳지 않는다.
            {
                Hungry();
                Thirsty();
                ConstantDecreaseHP();
                GaugeUpdate();
                SPRechargeTime();
                SPRecover();
            }
        }
    }

    private void Hungry()
    {
        if (currentHungry > 0)
        {
            if (currentHungryDecreaseTime <= hungryDecreaseTime) //헝그리감소타임 이될때까지 현재헝그리감소타임을 증가
                currentHungryDecreaseTime++;
            else
            {
                //정해진 헝그리 감소타임을 넘게되면 헝그리게이지 감소
                currentHungry--;
                //그후 다시계산을 위해 현재헝그리감소타임을 초기화
                currentHungryDecreaseTime = 0;
            }
        }
        else
            Debug.Log("배고픔 수치가 0이 되었습니다."); //배고픔수치가 0이되었을때 이벤트 실행예정
    }

    
    private void Thirsty()
    {
        if (currentThirsty > 0)
        {
            if (currentThirstyDecreaseTime <= thirstyDecreaseTime) 
                currentThirstyDecreaseTime++;
            else
            {
                currentThirsty--;
                currentThirstyDecreaseTime = 0;
            }
        }
        else
            Debug.Log("목마름 수치가 0이 되었습니다."); //목마름수치가 0이되었을때 이벤트 실행예정
    }

    private void GaugeUpdate()
    {
        //fillAmount는 백분율이므로 현재값을 설정한 최대값으로 나눈값을 넣어줌. (ex : 현재체력 / 최대체력)
        imagesGauge[HP].fillAmount = (float)currentHp / hp;
        imagesGauge[SP].fillAmount = (float)currentSp / sp;
        imagesGauge[DP].fillAmount = (float)currentDp / dp;
        imagesGauge[HUNGRY].fillAmount = (float)currentHungry / hungry;
        imagesGauge[THIRSTY].fillAmount = (float)currentThirsty / thirsty;
        imagesGauge[SATISFY].fillAmount = (float)currentSatisfy / satisfy;
    }

    public void IncreaseHP(int _count)
    {

        if (currentHp + _count < hp) //현재hp+회복될hp량이 현재체력보다 작으면
            currentHp += _count; //회복량만큼 hp회복
        else //넘으면
            currentHp = hp; //최대hp넘기지않게 최대hp량으로 만들어줌
    }

    public void IncreaseSP(int _count)
    {
        if (currentSp + _count < sp) 
            currentSp += _count; 
        else 
            currentSp = sp; 
    }
    
    //순간적인 HP가 닳을때
    public void DecreaseHP(int _count)
    {
        //방어력이 존재할경우 방어력을 체력대신깎고 종료.
        /*
        if(currentDp>0)
        {
            DecreaseDP(_count);
            return;
        }
        */
        currentHp -= _count;

        if (currentHp <= 0)
        {
            Debug.Log("캐릭터의 hp가 0이 되었습니다!!!"); //이때 게임오버 이벤트 넣을예정.
            theGameOver.reasonText.text = "체력이 0이되어 사망하였습니다..";
            GameOver.isGameOver = true;
        }
    }



    //지속적으로 HP가 닳을때
    public void ConstantDecreaseHP()
    {
        if (currentHp > 0)
        {
            //배고픔이나 목마름 수치가 0이되면 체력감소
            if (currentHungry <= 0 || currentThirsty <= 0)
            {
                if (currentHpDecreaseTime <= HpDecreaseTime)
                    currentHpDecreaseTime++;
                else
                {
                    currentHp--;
                    currentHpDecreaseTime = 0;
                }
            }
        }
        else
        {
            theGameOver.reasonText.text = "체력이 0이되어 사망하였습니다..";
            GameOver.isGameOver = true;
            
            Debug.Log("캐릭터의 hp가 0이 되었습니다!!!"); //이때 게임오버 이벤트 넣을예정.
        }
    }

    //public void DecreaseSP(int _count)
    //{
    //    currentSp -= _count;
    //
    //    //if (currentSp <= 0)
    //       //Debug.Log("스테미나가 0이 되었습니다.");
    //}

    public void DecreaseStamina(int _count)
    {
        spUsed = true; //sp가 줄었으므로 트루
        currentSpRechargeTime = 0;

        if (currentSp - _count > 0)
            currentSp -= _count;
        else
            currentSp = 0;
    }

    public void IncreaseDP(int _count)
    {
        if (currentDp + _count < dp) 
            currentDp += _count;
        else 
            currentDp = dp;
    }

    public void DecreaseDP(int _count)
    {
        currentDp -= _count;

        if (currentDp <= 0)
            Debug.Log("방어력이 0이 되었습니다.");
    }


    public void IncreaseHungry(int _count)
    {
        if (currentHungry + _count < hungry)
            currentHungry += _count;
        else
            currentHungry = hungry;
    }

    public void DecreaseHungry(int _count)
    {
        if (currentHungry - _count < 0)
            currentHungry = 0;
        else
            currentHungry -= _count;
    }

    public void IncreaseThirsty(int _count)
    {
        if (currentThirsty + _count < thirsty)
            currentThirsty += _count;
        else
            currentThirsty = thirsty;
    }

    public void DecreaseThirsty(int _count)
    {
        if (currentThirsty - _count < 0)
            currentThirsty = 0;
        else
            currentThirsty -= _count;
    }

    public void IncreaseSatisfy(int _count)
    {
        if (currentSatisfy + _count < satisfy)
            currentSatisfy += _count;
        else
            currentSatisfy = satisfy;
    }

    public void DecreaseSatisfy(int _count)
    {
        if (currentSatisfy - _count < 0)
            currentSatisfy = 0;
        else
            currentSatisfy -= _count;
    }

    public int GetCurrentSP()
    {
        return currentSp;
    }

    private void SPRechargeTime()
    {
        if(spUsed)
        {
            if (currentSpRechargeTime < spRechargeTime)
                currentSpRechargeTime++;
            else
                spUsed = false;
        }
    }

    private void SPRecover()
    {
        if(!spUsed && currentSp<sp) //sp가 더이상 사용상태아니고 현재sp가 최대sp보다 작을때 회복실행
        {
            currentSp += spIncreaseSpeed;
        }
    }
}
