using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    //현재 무기와 현재무기의 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    //현재 무기의 타입.
    //[SerializeField]
    private string currentWeaponType = "HAND"; //맨처음 시작무기는 맨손

    //무기 중복 교체 실행 방지.
    public static bool isChangeWeapon;

    //무기 얻음 상태변수
    private bool isGetAxe;
    private bool isGetPickAxe;
    private bool isGetSubMachineGun1;

    //무기 교체 딜레이.
    [SerializeField]
    private float changeWeaponDelayTime;
    //무기 교체가 완전히 끝난 시점.
    [SerializeField]
    private float changeWeaponEndDelayTime;
    
    //무기 종류들 전부 관리.
    [SerializeField]
    private CloseWeapon[] hands;
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] axes;
    [SerializeField]
    private CloseWeapon[] pickaxes;

    //관리 차원에서 무기를 이름으로 쉽게 접근하도록 만듦.
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    //사용예
    //closeWeaponDictionary.Add("맨손", closeWeapons[0]); //딕셔너리에 저장
    //closeWeaponDictionary["맨손"]; //저장된것을 꺼낼때 이렇게 사용

    //필요한 컴포넌트 (서로 번갈아 껐다켰다 할예정)
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickAxeController thePickaxeController;

    private HUD theHUD;

    // Use this for initialization
    void Start ()
    {
        //근접무기(손) 이름값 초기화
        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }
        //근접무기(도끼) 이름값 초기화
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        //근접무기(곡괭이) 이름값 초기화
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }
        //총 초기화
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }

        theHUD = FindObjectOfType<HUD>();
    }

    //디버그용 타임변수(업데이트에서 2초마다 디버깅)
    //private float TimeLeft = 2.0f;
    //private float nextTime = 0.0f;

	// Update is called once per frame
	void Update ()
    {
		if(!isChangeWeapon)
        {
            //1을 눌렀을때 무기교체 실행(맨손)
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
                theHUD.goBulletHUD.SetActive(false);
            }
            //2을 눌렀을때 무기교체 실행(도끼)
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //못얻은 상태일시 임시 불변수를 이용하여 코루틴 실행전 종료.
                if (!isGetAxe)
                    return;
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe1"));

                theHUD.goBulletHUD.SetActive(false);
            }
            //3을 눌렀을때 무기교체 실행(총)
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (!isGetSubMachineGun1)
                    return;
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));

                theHUD.goBulletHUD.SetActive(true);
            }
            //4을 눌렀을때 무기교체 실행(곡괭이)
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (!isGetPickAxe)
                    return;
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe1"));
                theHUD.goBulletHUD.SetActive(false);
            }
        }

        //디버그용
        /*
        if (Time.time > nextTime)
        {
            nextTime = Time.time + TimeLeft;
            Debug.Log("핸드컨트롤러 작동여부 : " + HandController.isActivate);
            Debug.Log("건컨트롤러 작동여부 : " + GunController.isActivate);
        }
        */
	}

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name) //바꿀타입무기, 어떤무기로 바꿀지
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction(); //이전 무기상태 해제 (ex:정조준상태 해제)
        WeaponChange(_type,_name); //원하는 무기로 교체

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type; //현재 무기타입을 바꾼 무기타입으로 다시설정.
        isChangeWeapon = false; //무기교체가 끝나면 다시 false로바꿔서 무기교체가 가능하도록 바꿔줌
    }

    private void CancelPreWeaponAction()
    {
        switch(currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "HAND":
                HandController.isActivate = false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
            case "PICKAXE":
                PickAxeController.isActivate = false;
                break;
        }
    }

    private void WeaponChange(string _type, string _name)
    {
        if(_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        else if(_type == "HAND")
             theHandController.CloseWeaponChange(handDictionary[_name]);
        else if (_type == "AXE")
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
        else if (_type == "PICKAXE")
            thePickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
    }

    public void IsChangeGetAxe()
    {
        isGetAxe = !isGetAxe;
    }

    public void IsChangeGetPickAxe()
    {
        isGetPickAxe = !isGetPickAxe;
    }

    public void IsChangeGetSubMachineGun1()
    {
        isGetSubMachineGun1 = !isGetSubMachineGun1;
    }
    
}
