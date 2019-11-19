using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandController : CloseWeaponController
{
    // 활성화 여부.
    public static bool isActivate = true;

    void Start()
    {
        //핸드상태(맨손)일때 맨처음 게임시작하므로 핸드값으로 초기화
        WeaponManager.currentWeapon = currentCloseWeapon.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentCloseWeapon.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
            TryAttack();
    }

    protected override IEnumerator HitCoroutine()
    {

        //이 코루틴은 isSwing이 true일때만 실행됨 false되면 자동종료
        while (isSwing)
        {
            if (CheckObject()) //충돌했음
            {
                //동물일경우
                if (hitInfo.transform.tag == "NPC")
                {
                    //동물에게 일정데미지 입힘
                    //hitInfo.transform.GetComponent<Rhino>().Damage(currentCloseWeapon.damage,transform.position); //도끼의 데미지와 도끼의 현재위치를 넘겨줌
                    SoundManager.instance.PlaySE("Animal_Hit");
                    hitInfo.transform.GetComponent<Rhino>().Damage(1, transform.position); //실험용 데미지1
                }

                isSwing = false; //적중앙에 있을때 중복대미지가 들어가는 것을 방지하기위해 isSwing을 꺼줌.

                //디버그 텍스트
                Debug.Log(hitInfo.transform.name);
                //debugText.gameObject.SetActive(true);
                //debugText.text = hitInfo.transform.name + "<color=yellow>" + " 히트! " + "</color>";
                yield return new WaitForSeconds(0.5f);
            }
            yield return null; //while문 한번돌동안 1프레임대기
        }
        //debugText.gameObject.SetActive(false);
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }

    /*
    // 활성화 여부.
    public static bool isActivate = true;

    [SerializeField]
    private Text debugText;


    //현재 장착된 Hand형 타입 무기.
    [SerializeField]
    private CloseWeapon currentHand;

    //공격 상태변수
    private bool isAttack = false;
    private bool isSwing = false;

    //광선을 쐈을때 닿은 물체의 정보
    private RaycastHit hitInfo;

    void Start()
    {
        //핸드상태(맨손)일때 맨처음 게임시작하므로 핸드값으로 초기화
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivate)
           TryAttack();
	}

    private void TryAttack()
    {
        //마우스 좌클릭(계속꽉눌러도 처리)
        if(Input.GetButton("Fire1"))
        {
            if(!isAttack)
            {
                //코루틴 실행.
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;

        //어택애니메이션 트리거 작동
        currentHand.anim.SetTrigger("Attack");

        //공격활성화 적중되기 전까지 딜레이
        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true; //스윙값이 true가 된순간 공격이 적중됐는지 체크


        StartCoroutine(HitCoroutine()); //isSwing이 true값인 동안 충돌체크 계속반복

        //공격비활성화(팔을 접는동안)까지 딜레이
        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        //팔을 완전히 접으면 공격할수 있게 대기(이미 딜레이값이 된것들은 뺀나머지 시간동안대기)
        yield return new WaitForSeconds(currentHand.attackDelay- currentHand.attackDelayA - currentHand.attackDelayB);

        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        //이 코루틴은 isSwing이 true일때만 실행됨 false되면 자동종료
        while(isSwing)
        {
            if(CheckObject()) //충돌했음
            {
                isSwing = false; //적중앙에 있을때 중복대미지가 들어가는 것을 방지하기위해 isSwing을 꺼줌.
                Debug.Log(hitInfo.transform.name);
                debugText.gameObject.SetActive(true);
                debugText.text = hitInfo.transform.name + "<color=yellow>" + " 히트! " + "</color>";
                yield return new WaitForSeconds(0.5f);
            }
            yield return null; //while문 한번돌동안 1프레임대기
        }
        debugText.gameObject.SetActive(false);
    }
    private bool CheckObject()
    {
        //레이를 캐릭터 앞쪽 방향으로 쏴서 맞은물체가 있으면 true
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, currentHand.range))
        {
            return true;
        }
        return false;
    }

    public void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
            WeaponManager.currentWeapon.gameObject.SetActive(false);

        currentHand = _closeWeapon;
        WeaponManager.currentWeapon = currentHand.GetComponent<Transform>();
        WeaponManager.currentWeaponAnim = currentHand.anim;

        currentHand.transform.localPosition = Vector3.zero;
        currentHand.gameObject.SetActive(true);
        isActivate = true;
    }
    */
}
